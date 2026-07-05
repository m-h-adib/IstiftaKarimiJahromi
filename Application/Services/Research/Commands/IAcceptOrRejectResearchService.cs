using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.bases.Commands;
using Application.Services.Process.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Research.Commands;

public interface IAcceptOrRejectResearchService
{
    Task<ResultDto> ExecuteAsync(AcceptOrRejectResearchDto dto);
}

public class AcceptOrRejectResearchService: IAcceptOrRejectResearchService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISendMessageService _sendMessageService;
    #endregion
    #region ctors
    public AcceptOrRejectResearchService(IDatabaseContext context,
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


    public async Task<ResultDto> ExecuteAsync(AcceptOrRejectResearchDto dto)
    {
        #region validation
        var validator = new AcceptOrRejectResearchValidation().Validate(dto);
        if (!validator.IsValid)
            return ResultDto.Failed(validator.ErrorMessage());
        #endregion

        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "AcceptResearchQuestion"))
            return ResultDto.Failed("NOT ACCESS", 403);

       

        var cycle = await _context.QuestionCycles
            .Where(p => p.Id == dto.CycleId)
            .Where(p => !p.IsRemoved)
			.Where(p => p.IsAccept == null)
			.Include(p => p.Question)
            .FirstOrDefaultAsync();

        if (cycle == null)
            return ResultDto.Failed("تحقیق مورد نظر یافت نشد");

		var research = await _context.Researchs
			.Where(p => p.QuestionCycleId == cycle.Id)
			.Where(p => !p.IsRemoved)
			.FirstOrDefaultAsync();

		if (research == null)
			return ResultDto.Failed("هنوز تحقیق برای این سوال ثبت نشده");

        if (await _context.Sessions
            .AnyAsync(p => !p.IsRemoved && p.IsActive && p.ResearchId==research.Id && p.Date.Value.AddDays(1).CompareTo(DateTime.Now)>=0))
            return ResultDto.Failed("شما یک جلسه فعال برای این تحقیق دارید و تا فردای برگزاری جلسه نمیتوانید تحقیق را تایید و یا رد کنید");

        cycle.IsAccept = dto.IsAccept;
        cycle.UpdatedAt = DateTime.Now;
        research.IsAccept = dto.IsAccept;
        research.UpdatedAt = DateTime.Now;  

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
                    ToStep = Domain.Enums.QuestionStepType.Editor,
                    IsActive = true,
                    IsRemoved = false
                });

            cycle.Question!.Step = Domain.Enums.QuestionStepType.Editor;

            try
            {
                await _sendMessageService.ExecuteAsync(cycle.ToUserId!.Value, "با سلام" + Environment.NewLine + "محقق عزیز تحقیق سوال شما با عنوان " + cycle.Question.Title + " تایید شد." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");

                await _sendMessageService.ExecuteAsync(dto.ToUser!.Value, "با سلام" + Environment.NewLine + "ویراستار عزیز یک سوال جهت ویراستاری به پنل شما اضافه گردید." + Environment.NewLine + "لطفا در اسرع وقت رسیدگی نمایید." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
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

                await _sendMessageService.ExecuteAsync(cycle.ToUserId!.Value, "با سلام" + Environment.NewLine + "محقق عزیز تحقیق سوال شما با عنوان " + cycle.Question.Title + " مورد تایید قرار نگرفت و جهت بازبینی مجدد ارجاع داده شد." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
            }
            catch (Exception ex) { }
        }

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}

#region dtos
public class AcceptOrRejectResearchDto
{
    public long CycleId { get; set; }
    public string? Description { get; set; }
    public long? ToUser { get; set; }
    public bool IsAccept { get; set; }
}
#endregion

#region validation
public class AcceptOrRejectResearchValidation : AbstractValidator<AcceptOrRejectResearchDto>
{
    public AcceptOrRejectResearchValidation()
    {
        RuleFor(p => p.CycleId).NotNull().WithMessage("لطفا شماره تحقیق را وارد نمایید");
        RuleFor(p => p.Description).NotEmpty().WithMessage("لطفا توضیحات را وارد نمایید");
        RuleFor(p => p.ToUser).NotNull().WithMessage("لطفا کاربر مورد نظر را انتخاب نمایید");
    }
}
#endregion