using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Archive.Queries;

public interface IGetQuestionResultService
{
    Task<ResultDto<List<GetQuestionResultDto>>> ExecuteAsync(long? questionId);
}

public class GetQuestionResultService: IGetQuestionResultService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetQuestionResultService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<List<GetQuestionResultDto>>> ExecuteAsync(long? questionId)
    {
        if (questionId == null)
            return ResultDto<List<GetQuestionResultDto>>.Failed("Bad Request");

        var res = await _context.Editors
            .Where(p => p.QuestionCycle!.QuestionId == questionId)
            .Where(p => p.IsActive)
            .Where(p => !p.IsRemoved)
            .Where(p => !p.QuestionCycle!.IsRemoved)
            .Where(p => !p.QuestionCycle!.Question!.IsRemoved)
            .Select(p => new GetQuestionResultDto
            {
                Language = p.Language!.Title,
                Answer = p.AnswerText,
                Reason = p.ReasonText,
                IsAccept = p.IsAccept,
                Description = p.Description,
                Date = p.CreatedAt.ToPersianWithoutTime(),
                EditorUser = p.QuestionCycle!.ToUserId != null ? p.QuestionCycle!.ToUser!.Name : ""
            })
            .ToListAsync();

        return ResultDto<List<GetQuestionResultDto>>.Succeeded("ok", res);
    }
}

#region dtos
public class GetQuestionResultDto
{
    public string? Language { get; set; }
    public string? Answer { get; set; }
    public string? Reason { get; set; }
    public bool? IsAccept { get; set; }
    public string? Description { get; set; }
    public string? Date { get; set; }
    public string? EditorUser { get; set; }
}
#endregion