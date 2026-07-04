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
using Domain.Enums;

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

        var items = _context.QuestionCycles
            .Where(p => !p.IsRemoved)
            .Where(p => p.QuestionId == dto.QuestionId)
            .Where(p => p.ToStep == Domain.Enums.QuestionStepType.Editor);

		if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "ShowAllEditor"))
			items = items.Where(p => p.ToUserId == userid && p.ToStep == QuestionStepType.Editor);
		
        
        var res = await items
			.Select(p => new GetEditorCycleDto
			{
                Id = p.Id,
                DuplicateId = _context.PreProccesses.Any(u => u.DuplicateId != null && u.QuestionCycle.QuestionId == p.QuestionId && !u.IsRemoved && !u.QuestionCycle.IsRemoved) ? _context.PreProccesses.OrderBy(u => u.Id).Where(u => u.DuplicateId != null && u.QuestionCycle.QuestionId == p.QuestionId && !u.IsRemoved && !u.QuestionCycle.IsRemoved).LastOrDefault().DuplicateId : null,
                Duplicate = _context.PreProccesses.Any(u => u.DuplicateId != null && u.QuestionCycle.QuestionId == p.QuestionId && !u.IsRemoved && !u.QuestionCycle.IsRemoved) ? _context.PreProccesses.OrderBy(u => u.Id).Where(u => u.DuplicateId != null && u.QuestionCycle.QuestionId == p.QuestionId && !u.IsRemoved && !u.QuestionCycle.IsRemoved).Select(u => u.Duplicate.Title).LastOrDefault() : null,//p.PreProccess != null && p.PreProccess.DuplicateId != null ? p.PreProccess.Duplicate!.Title : null,
                IsAccept = p.IsAccept,
                QDesctiption = p.Description,
				HasProccess = p.Editors!=null && p.Editors.Any() ? true : false,
                Name=p.ToUserId!=null ? p.ToUser!.Name : "",
                Items=p.Editors!=null && p.Editors.Any() ? p.Editors!.Select(u=>new GetEditorCycleItemDto
                {
                    AnswerText = u.AnswerText,
                    Description = u.Description,
                    IsAccept=u.IsAccept,
                    Language=u.Language!.Title,
                    LanguageId=u.LanguageId,
                    ReasonText=u.ReasonText,
					Name = p.FromUserId != null ? p.FromUser!.Name : ""
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
    public long? DuplicateId { get; set; }
    public string? Duplicate { get; set; }
    public string? QDesctiption { get; set; }
	public bool HasProccess { get; set; }
	public bool? IsAccept { get; set; }
    public string? Name { get; set; }
    public ICollection<GetEditorCycleItemDto>? Items { get; set; }
}
public class GetEditorCycleItemDto
{
	public string? AnswerText { get; set; }
	public string? ReasonText { get; set; }
	public bool? IsAccept { get; set; }
	public string? Description { get; set; }
    public string? Language { get; set; }
    public long? LanguageId { get; set; }
    public string? Name { get; set; }
}
#endregion