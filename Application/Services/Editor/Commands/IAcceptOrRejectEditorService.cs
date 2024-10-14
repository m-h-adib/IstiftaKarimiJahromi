using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Editor.Commands;

public interface IAcceptOrRejectEditorService
{
    Task<ResultDto> ExecuteAsync(AcceptOrRejectEditorDto dto);
}

public class AcceptOrRejectEditorService : IAcceptOrRejectEditorService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public AcceptOrRejectEditorService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion


    public async Task<ResultDto> ExecuteAsync(AcceptOrRejectEditorDto dto)
    {
        #region validation
        var validator = new AcceptOrRejectEditorValidation().Validate(dto);
        if (!validator.IsValid)
            return ResultDto.Failed(validator.ErrorMessage());
        #endregion

        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "AcceptEditor"))
            return ResultDto.Failed("NOT ACCESS", 403);

        var cycle = await _context.QuestionCycles
            .Where(p => p.Id == dto.CycleId)
            .Where(p => !p.IsRemoved)
            .Include(p => p.Question)
            .FirstOrDefaultAsync();

        if (cycle == null)
            return ResultDto.Failed("ادیت مورد نظر یافت نشد");

		var editor = await _context.Editors
			.Where(p => p.QuestionCycleId == cycle.Id)
			.Where(p => !p.IsRemoved)
			.FirstOrDefaultAsync();

		if (editor == null)
			return ResultDto.Failed("هنوز ویراستاری برای این سوال ثبت نشده");

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
                    ToStep = Domain.Enums.QuestionStepType.Archive,
                    IsActive = true,
                    IsRemoved = false
                });

            cycle.Question!.Step = Domain.Enums.QuestionStepType.Archive;
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
        }

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}

#region dtos
public class AcceptOrRejectEditorDto
{
    public long CycleId { get; set; }
    public string? Description { get; set; }
    public long? ToUser { get; set; }
    public bool IsAccept { get; set; }
}
#endregion

#region validation
public class AcceptOrRejectEditorValidation : AbstractValidator<AcceptOrRejectEditorDto>
{
    public AcceptOrRejectEditorValidation()
    {
        RuleFor(p => p.CycleId).NotNull().WithMessage("لطفا شماره ادیت را وارد نمایید");
        RuleFor(p => p.Description).NotEmpty().WithMessage("لطفا توضیحات را وارد نمایید");
       // RuleFor(p => p.ToUser).NotNull().WithMessage("لطفا کاربر مورد نظر را انتخاب نمایید");
    }
}
#endregion