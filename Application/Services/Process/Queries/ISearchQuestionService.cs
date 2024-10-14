using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Process.Queries;

public interface ISearchQuestionService
{
	Task<ResultDto<List<SearchQuestionDto>>> ExecuteAsync(SearchQuestionRequestDto dto);
}

public class SearchQuestionService : ISearchQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public SearchQuestionService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
	}
	#endregion


	public async Task<ResultDto<List<SearchQuestionDto>>> ExecuteAsync(SearchQuestionRequestDto dto)
	{

		
		var items = _context.Questions
			.Where(p => !p.IsRemoved)
			.Where(p => p.Step == Domain.Enums.QuestionStepType.Archive);

		if (!string.IsNullOrEmpty(dto.Title))
		{
			List<string> keys = dto.Title.Split(" ").ToList();

			items = items.Where(p => p.Title!.Contains(dto.Title) || p.QuestionCategory!.Title!.Contains(dto.Title) || p.QuestionText!.Contains(dto.Title) || keys.Any(u => u.Equals(p.Title)));
		}

		if (dto.Category != null)
			items = items.Where(p => p.QuestionCategoryId == dto.Category);

		var res = await items
			.Select(p => new SearchQuestionDto
			{
				Id = p.Id,
				Title = p.Title,
				Text = p.QuestionText,
				Category = p.QuestionCategory!.Title
			})
			.ToListAsync();


		return ResultDto<List<SearchQuestionDto>>.Succeeded("ok", res);

	}
}

#region dtos
public class SearchQuestionRequestDto
{
	public string? Title { get; set; }
	public long? Category { get; set; }
}
public class SearchQuestionDto
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? Text { get; set; }
	public string? Category { get; set; }
}
#endregion