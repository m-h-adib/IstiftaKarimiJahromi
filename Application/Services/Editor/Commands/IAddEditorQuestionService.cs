using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Editor.Commands;

public interface IAddEditorQuestionService
{
    Task<ResultDto> ExecuteAsync(AddEditorQuestionDto dto);
}
public class AddEditorQuestionService : IAddEditorQuestionService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostingEnvironment _environment;
    #endregion
    #region ctors
    public AddEditorQuestionService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor,
        IHostingEnvironment hostingEnvironment)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
        _environment = hostingEnvironment;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddEditorQuestionDto dto)
    {
        var cycle = await _context.QuestionCycles
            .Include(p => p.Editors)
            .FirstOrDefaultAsync(p => p.QuestionId == dto.QuestionId && !p.IsRemoved && p.IsAccept == null);

        if (cycle == null)
            return ResultDto.Failed("خطا در ارسال اطلاعات");

        if (cycle.Editors!=null && cycle.Editors.Any(p=>p.LanguageId==dto.LanguageId))
            return ResultDto.Failed("شما این سوال را ادیت کرده اید و دوباره مجاز به ادیت نمی باشید");

      

        await _context.Editors.AddAsync(new Domain.Entities.Editor
        {
            CreatedAt = DateTime.Now,
            AnswerText=dto.AnswerText,
            ReasonText= dto.ReasonText,
            Description = dto.Description,
            IsActive = true,
            IsRemoved = false,
            QuestionCycleId = cycle.Id,
            LanguageId= dto.LanguageId
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ادیت سوال و جواب انجام شد");
    }
}
#region dtos
public class AddEditorQuestionDto
{
    public long? QuestionId { get; set; }
    public string? Description { get; set; }
    public string? AnswerText { get; set; }
    public string? ReasonText { get;set; }
	public long? LanguageId { get; set; }
}
#endregion