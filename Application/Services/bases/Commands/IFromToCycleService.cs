using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Commands;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IFromToCycleService
{
    Task<ResultDto> ExecuteAsync(FromToCycleDto dto);
}
public class FromToCycleService: IFromToCycleService
{
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISendMessageService _sendMessageService;
    public FromToCycleService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor,
        ISendMessageService sendMessageService)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
        _sendMessageService = sendMessageService;
    }

    public async Task<ResultDto> ExecuteAsync(FromToCycleDto dto)
    {
        #region validation
        var validator = new FromToCycleValidation().Validate(dto);
        if (!validator.IsValid)
            return ResultDto.Failed(validator.ErrorMessage());
        #endregion

        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);


        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection.Name == "FromToCycle"))
            return ResultDto.Failed("NOT ACCESS", 403);


        var cycle = await _context.QuestionCycles
            .Where(p => p.Id == dto.CycleId)
            .Where(p => !p.IsRemoved)
            .Include(p => p.Question)
            .FirstOrDefaultAsync();



        if (cycle == null)
            return ResultDto.Failed("پردازش مورد نظر یافت نشد");


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
                  ToStep = (Domain.Enums.QuestionStepType)dto.ToStep.Value,
                  IsActive = true,
                  IsRemoved = false
              });

        cycle.Question!.Step = (Domain.Enums.QuestionStepType)dto.ToStep.Value;

        foreach (var item in _context.QuestionCycles.Where(p=>p.QuestionId==cycle.QuestionId && p.ToStep>(QuestionStepType)dto.ToStep.Value && p.IsAccept==null))
        {
            item.IsAccept = false;
            item.Description = dto.Description;
        }

        // await _sendMessageService.ExecuteAsync(cycle.ToUserId!.Value, "با سلام" + Environment.NewLine + "پردازشگر عزیز پردازش سوال شما با عنوان " + cycle.Question.Title + " تایید شد." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
        //await _sendMessageService.ExecuteAsync(dto.ToUser!.Value, "با سلام" + Environment.NewLine + "محقق عزیز یک سوال جهت تحقیق به پنل شما اضافه گردید." + Environment.NewLine + "لطفا در اسرع وقت رسیدگی نمایید." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");



        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}
#region dtos
public class FromToCycleDto
{
    public long CycleId { get; set; }
    public string? Description { get; set; }
    public long? ToUser { get; set; }
    public int? ToStep { get; set; }
}
#endregion

#region validation
public class FromToCycleValidation : AbstractValidator<FromToCycleDto>
{
    public FromToCycleValidation()
    {
        RuleFor(p => p.CycleId).NotNull().WithMessage("لطفا شماره پردازش را وارد نمایید");
        RuleFor(p => p.Description).NotEmpty().WithMessage("لطفا توضیحات را وارد نمایید");
        RuleFor(p => p.ToUser).NotNull().WithMessage("لطفا کاربر مورد نظر را انتخاب نمایید");
        RuleFor(p => p.ToStep).NotNull().WithMessage("لطفا بخش مورد نظر را انتخاب نمایید");
    }
}
#endregion