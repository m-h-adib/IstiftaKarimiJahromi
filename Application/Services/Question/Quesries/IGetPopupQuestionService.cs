using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Question.Quesries;

public interface IGetPopupQuestionService
{
    Task<ResultDto<GetPopupQuestionDto>> ExecuteAsync(long? QuestionId);
}

public class GetPopupQuestionService: IGetPopupQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetPopupQuestionService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<GetPopupQuestionDto>> ExecuteAsync(long? QuestionId)
    {
        if (QuestionId == null)
            return ResultDto<GetPopupQuestionDto>.Failed("bad request");

        var question=await _context.Questions
            .Where(p=>!p.IsRemoved)
            .Where(p=>p.Id==QuestionId)
            .Select(p=>new GetPopupQuestionDto
            {
                Title=p.Title,
                Attach=p.QuestionAttaches!=null ? p.QuestionAttaches
                .Select(u=>u.File).ToList() : null,
                Question=p.QuestionText,
                QuestionFile=p.QuestionAttaches.Any()? p.QuestionAttaches.FirstOrDefault().File: "",
                Category=p.QuestionCategory!.Title,
                Answer=p.QuestionCycles.Any(u=>u.Research!=null && u.ToStep==Domain.Enums.QuestionStepType.Research) ?
                p.QuestionCycles.Where(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research).Select(u=>u.Research).OrderBy(u=>u.Id).LastOrDefault().AnswerText : "",
                AnswerFile = p.QuestionCycles.Any(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research) ?
                p.QuestionCycles.Where(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research).Select(u => u.Research).OrderBy(u => u.Id).LastOrDefault().AnswerFile : "",
                Reason = p.QuestionCycles.Any(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research) ?
                p.QuestionCycles.Where(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research).Select(u => u.Research).OrderBy(u => u.Id).LastOrDefault().ReasonText : "",
                ReasonFile = p.QuestionCycles.Any(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research) ?
                p.QuestionCycles.Where(u => u.Research != null && u.ToStep == Domain.Enums.QuestionStepType.Research).Select(u => u.Research).OrderBy(u => u.Id).LastOrDefault().ReasonFile : ""
            })
            .FirstOrDefaultAsync();

        if (question == null)
            return ResultDto<GetPopupQuestionDto>.Failed("سوال یافت نشد");


        return ResultDto<GetPopupQuestionDto>.Succeeded("ok", question);


    }
}
#region dtos
public class GetPopupQuestionDto
{
    public string? Category { get; set; }
    public string? Title { get; set; }
    public string? Question { get; set; }
    public string? QuestionFile { get; set; }
    public string? Answer { get; set; }
    public string? AnswerFile { get; set; }
    public string? Reason { get; set; }
    public string? ReasonFile { get; set; }
    public List<string>? Attach { get; set; }
}
#endregion