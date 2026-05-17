using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.bases.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Process.Commands;

public interface IAcceptOrRejectProcessService
{
	Task<ResultDto> ExecuteAsync(AcceptOrRejectProcessDto dto);
}

public class AcceptOrRejectProcessService : IAcceptOrRejectProcessService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISendMessageService _sendMessageService;
    #endregion
    #region ctors
    public AcceptOrRejectProcessService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor,
        ISendMessageService sendMessageService)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
        _sendMessageService = sendMessageService;
    }
	#endregion


	public async Task<ResultDto> ExecuteAsync(AcceptOrRejectProcessDto dto)
	{
		#region validation
		var validator = new AcceptOrRejectProcessValidation().Validate(dto);
		if (!validator.IsValid)
			return ResultDto.Failed(validator.ErrorMessage());
		#endregion

		long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);


		if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection.Name == "AcceptProcessQuestion"))
			return ResultDto.Failed("NOT ACCESS", 403);


		var cycle = await _context.QuestionCycles
			.Where(p => p.Id == dto.CycleId)
			.Where(p => !p.IsRemoved)
			.Where(p => p.IsAccept == null)
			.Include(p => p.Question)
			.FirstOrDefaultAsync();



		if (cycle == null)
			return ResultDto.Failed("پردازش مورد نظر یافت نشد");

		var proccess = await _context.PreProccesses
			.Where(p => p.QuestionCycleId == cycle.Id)
			.Where(p => !p.IsRemoved)
			.FirstOrDefaultAsync();

		if (proccess == null)
			return ResultDto.Failed("هنوز پردازشی برای این سوال ثبت نشده");

		cycle.IsAccept = dto.IsAccept;
		cycle.UpdatedAt = DateTime.Now;

		if (dto.IsAccept == true)
		{
			await _context.QuestionCycles
				.AddAsync(new Domain.Entities.QuestionCycle
				{
					FromUserId = cycle.ToUserId,
					UserId = userid,
					ToUserId = dto.ToUser,
					QuestionId = cycle.QuestionId,
					CreatedAt = DateTime.Now,
					Description = dto.Description,
					FromStep = cycle.ToStep,
					ToStep = Domain.Enums.QuestionStepType.Research,
					IsActive = true,
					IsRemoved = false
				});

			cycle.Question!.Step = Domain.Enums.QuestionStepType.Research;
			if(proccess.DuplicateId!=null)
				cycle.Question.DuplicateId = proccess.DuplicateId;


			try
			{
				await _sendMessageService.ExecuteAsync(cycle.ToUserId!.Value, "با سلام" + Environment.NewLine + "پردازشگر عزیز پردازش سوال شما با عنوان " + cycle.Question.Title + " تایید شد." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
				await _sendMessageService.ExecuteAsync(dto.ToUser!.Value, "با سلام" + Environment.NewLine + "محقق عزیز یک سوال جهت تحقیق به پنل شما اضافه گردید." + Environment.NewLine + "لطفا در اسرع وقت رسیدگی نمایید." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
			}
			catch (Exception ex) { }

        }
        else
		{
			await _context.QuestionCycles
			   .AddAsync(new Domain.Entities.QuestionCycle
			   {
				   FromUserId = cycle.ToUserId,
				   UserId = userid,
				   ToUserId = dto.ToUser,
				   QuestionId = cycle.QuestionId,
				   CreatedAt = DateTime.Now,
				   Description = dto.Description,
				   FromStep = cycle.ToStep,
				   ToStep = cycle.ToStep,
				   IsActive = true,
				   IsRemoved = false
			   });

			try
			{
				await _sendMessageService.ExecuteAsync(cycle.ToUserId!.Value, "با سلام" + Environment.NewLine + "پردازشگر عزیز پردازش سوال شما با عنوان " + cycle.Question.Title + " مورد تایید قرار نگرفت و جهت بازبینی مجدد ارجاع یافت." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
			}
			catch (Exception ex) { }
        }

        await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}

#region dtos
public class AcceptOrRejectProcessDto
{
	public long CycleId { get; set; }
	public string? Description { get; set; }
	public long? ToUser { get; set; }
	public bool IsAccept { get; set; }
}
#endregion

#region validation
public class AcceptOrRejectProcessValidation : AbstractValidator<AcceptOrRejectProcessDto>
{
	public AcceptOrRejectProcessValidation()
	{
		RuleFor(p => p.CycleId).NotNull().WithMessage("لطفا شماره پردازش را وارد نمایید");
		RuleFor(p => p.Description).NotEmpty().WithMessage("لطفا توضیحات را وارد نمایید");
		RuleFor(p => p.ToUser).NotNull().WithMessage("لطفا کاربر مورد نظر را انتخاب نمایید");
	}
}
#endregion