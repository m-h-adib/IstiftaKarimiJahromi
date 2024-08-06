using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Archive.Queries;

public interface IGetQuestionStepsService
{
    Task<ResultDto<List<GetQuestionStepDto>>> ExecuteAsync(long? questionId);
}

public class GetQuestionStepsService: IGetQuestionStepsService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetQuestionStepsService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<List<GetQuestionStepDto>>> ExecuteAsync(long? questionId)
    {
        if (questionId == null)
            return ResultDto<List<GetQuestionStepDto>>.Failed("Bad Request");

        var q = await _context.Questions.FirstOrDefaultAsync(p => p.IsActive && !p.IsRemoved && p.Step == Domain.Enums.QuestionStepType.Archive && p.Id == questionId);
        if (q == null)
            return ResultDto<List<GetQuestionStepDto>>.Failed("سوالی یافت نشد");

        var res = await _context.QuestionCycles
            .Where(p => p.QuestionId == questionId)
            .Where(p => p.IsActive)
            .Where(p => !p.IsRemoved)
            .Select(p=>new GetQuestionStepDto
            {
                FromStep=p.FromStep!=null ? p.FromStep.GetDisplayName() : "",
                ToStep=p.ToStep!=null ? p.ToStep.GetDisplayName() : "",
                Description=p.Description,
                Date=p.CreatedAt.ToPersianWithoutTime(),
                IsAccept=p.IsAccept,
                FromUser=p.FromUserId!=null ? p.FromUser.Name : "",
                ToUser=p.ToUserId!=null ? p.ToUser.Name : "",
                CreateUser=p.UserId!=null ? p.User.Name : ""
            })
            .ToListAsync();


        return ResultDto<List<GetQuestionStepDto>>.Succeeded("ok", res);
    }
}

#region dtos
public class GetQuestionStepDto
{
    public string? FromStep { get; set; }
    public string? ToStep { get; set; }
    public string? Description { get; set; }
    public string? Date { get; set; }
    public bool? IsAccept { get; set; }
    public string? FromUser { get; set; }
    public string? ToUser { get; set; }
    public string? CreateUser { get; set; }
}
#endregion
