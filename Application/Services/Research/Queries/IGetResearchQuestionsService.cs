using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Quesries;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Research.Queries;

public interface IGetResearchQuestionsService
{
    Task<ResultDto<ResultOfList<GetResearchQuestionDto>>> ExecuteAsync(GetResearchQuestionRequestDto dto);
}

public class GetResearchQuestionsService: IGetResearchQuestionsService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetResearchQuestionsService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion


    public async Task<ResultDto<ResultOfList<GetResearchQuestionDto>>> ExecuteAsync(GetResearchQuestionRequestDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        var items = _context.Questions
            .Where(p => !p.IsRemoved && p.Step == QuestionStepType.Research);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "ShowAllResearchQuestion"))
            items = items.Where(p => p.QuestionCycles.Any(u => u.ToUserId == userid && u.ToStep == QuestionStepType.Research));


        #region Filters
        if (!dto.Title.IsNullOrEmpty())
            items = items.Where(p => p.Title!.Contains(dto.Title!));
        #endregion

        int rows = 0;
        var res = await items
            .ToPaged(dto.PageNumber, dto.PageSize, out rows)
            .Select(p => new GetResearchQuestionDto
            {
                Id = p.Id,
                Title = p.Title,
                Name = p.FirstName + " " + p.LastName,
                Category = p.QuestionCategory!.Title,
                QuestionDate = p.QuestionDate!.Value.ToPersian(),
                Text = p.QuestionText,
                Status = p.Step!.GetDisplayName(),
                UserId = p.UserId,
                UserName = p.User!.Name,
                IsChecked = _context.Researchs.Any(u => u.QuestionCycle!.QuestionId == p.Id && u.QuestionCycle.ToUserId == p.UserId && u.QuestionCycle.ToStep == QuestionStepType.Research && u.QuestionCycle.Research != null)
            })
            .ToListAsync();

        var result = new ResultOfList<GetResearchQuestionDto>
        {
            Items = res,
            Paging = new Paging(dto.PageNumber, dto.PageSize, rows)
        };

        return ResultDto<ResultOfList<GetResearchQuestionDto>>.Succeeded("ok", result);
    }
}

#region dtos
public class GetResearchQuestionRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Title { get; set; }
}
public class GetResearchQuestionDto
{
    public long? Id { get; set; }
    public string? Title { get; set; }
    public string? QuestionDate { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public bool? IsChecked { get; set; }
    public string? Text { get; set; }
    public string? Status { get; set; }
    public long? UserId { get; set; }
    public string? UserName { get; set; }
}
#endregion