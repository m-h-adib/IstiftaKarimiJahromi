using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
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

public class AcceptOrRejectProcessService: IAcceptOrRejectProcessService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public AcceptOrRejectProcessService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
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


        var cycle = await _context.QuestionCycles
            .Where(p => p.Id == dto.CycleId)
            .Where(p => !p.IsRemoved)
            .Include(p=>p.Question)
            .FirstOrDefaultAsync();

        if (cycle == null)
            return ResultDto.Failed("پردازش مورد نظر یافت نشد");

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