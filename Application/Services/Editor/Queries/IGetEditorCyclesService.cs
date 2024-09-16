using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Queries;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Editor.Queries;

public interface IGetEditorCyclesService
{
    Task<ResultDto<List<GetEditorCycleDto>>> ExecuteAsync(GetEditorCycleRequestDto dto);
}

public class GetEditorCyclesService : IGetEditorCyclesService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetEditorCyclesService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion


    public async Task<ResultDto<List<GetEditorCycleDto>>> ExecuteAsync(GetEditorCycleRequestDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "AddEditor"))
            return ResultDto<List<GetEditorCycleDto>>.Failed("NOT ACCESS", 403);

        var res = await _context.QuestionCycles
            .Where(p => !p.IsRemoved)
            .Where(p => p.ToUserId == userid)
            .Where(p => p.QuestionId == dto.QuestionId)
            .Where(p => p.ToStep == Domain.Enums.QuestionStepType.Editor)
            .Select(p => new GetEditorCycleDto
			{
                Id = p.Id,
                IsAccept = p.IsAccept,
                QDesctiption = p.Description,
                HasProccess = p.Editors!=null && p.Editors.Any() ? true : false,
                Items=p.Editors!=null && p.Editors.Any() ? p.Editors!.Select(u=>new GetEditorCycleItemDto
                {
                    AnswerText = u.AnswerText,
                    Description = u.Description,
                    IsAccept=u.IsAccept,
                    Language=u.Language!.Title,
                    ReasonText=u.ReasonText
                }).ToList() : null
            }).ToListAsync();

        return ResultDto<List<GetEditorCycleDto>>.Succeeded("ok", res);
    }
}
#region dtos
public class GetEditorCycleRequestDto
{
    public long QuestionId { get; set; }
}
public class GetEditorCycleDto
{
    public long Id { get; set; }
    public string? QDesctiption { get; set; }
    public bool HasProccess { get; set; }
	public bool? IsAccept { get; set; }
	public ICollection<GetEditorCycleItemDto>? Items { get; set; }
}
public class GetEditorCycleItemDto
{
	public string? AnswerText { get; set; }
	public string? ReasonText { get; set; }
	public bool? IsAccept { get; set; }
	public string? Description { get; set; }
    public string? Language { get; set; }
}
#endregion