using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Question.Quesries;

public interface IGetQuestionsService
{
    Task<ResultDto<ResultOfList<GetQuestionDto>>> ExecuteAsync(GetQuestionRequestDto dto);
}

public class GetQuestionsService: IGetQuestionsService
{
	#region members
	private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetQuestionsService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    public async Task<ResultDto<ResultOfList<GetQuestionDto>>> ExecuteAsync(GetQuestionRequestDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        var items = _context.Questions
            .Where(p => !p.IsRemoved && p.Step==QuestionStepType.Add);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.AdminSection!.Name == "ShowAllQuestions" && p.UserId == userid))
            items = items.Where(p => p.UserId == userid);


        #region Filters
        if (!dto.Title.IsNullOrEmpty())
            items = items.Where(p => p.Title!.Contains(dto.Title!));
        #endregion

        int rows = 0;
        var res = await items
            .ToPaged(dto.PageNumber, dto.PageSize, out rows)
            .Select(p => new GetQuestionDto
            {
                Id = p.Id,
                Title = p.Title,
                Name = p.FirstName + " " + p.LastName,
                Category = p.QuestionCategory.Title,
                QuestionDate = p.QuestionDate.Value.ToPersian(),
                Status = p.Step!.GetDisplayName()
            })
            .ToListAsync();

        var result = new ResultOfList<GetQuestionDto>
        {
            Items = res,
            Paging = new Paging(dto.PageNumber, dto.PageSize, rows)
        };

        return ResultDto<ResultOfList<GetQuestionDto>>.Succeeded("ok", result);
    }
}

#region dtos
public class GetQuestionRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Title { get; set; }
}
public class GetQuestionDto
{
    public long? Id { get; set; }
    public string? Title { get; set; }
	public string? QuestionDate { get; set; }
	public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
}
#endregion