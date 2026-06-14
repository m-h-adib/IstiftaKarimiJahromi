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
			.Where(p=>p.DuplicateId==null)
			.Where(p => p.Step == Domain.Enums.QuestionStepType.Archive);

		if (!string.IsNullOrEmpty(dto.Title))
		{
			List<string> keys = dto.Title.Split(" ").ToList();


			switch (keys.Count)
			{
				case 1:
					items = items.Where(p => p.Title.Contains(dto.Title) || dto.Title.Contains(p.Title) || p.QuestionText.Contains(dto.Title) || dto.Title.Contains(p.QuestionText) ||
					p.Title.Contains(keys[0]) || p.QuestionText.Contains(keys[0]) || keys[0].Contains(p.Title) || keys[0].Contains(p.QuestionText));
					break;
				case 2:
                    items = items.Where(p => p.Title.Contains(dto.Title) || dto.Title.Contains(p.Title) || p.QuestionText.Contains(dto.Title) || dto.Title.Contains(p.QuestionText) || 
					((p.Title.Contains(keys[0]) || p.QuestionText.Contains(keys[0]) || keys[0].Contains(p.Title) || keys[0].Contains(p.QuestionText)) &&
                    (p.Title.Contains(keys[1]) || p.QuestionText.Contains(keys[1]) || keys[1].Contains(p.Title) || keys[1].Contains(p.QuestionText))));
                    break;
                case 3:
                    items = items.Where(p => p.Title.Contains(dto.Title) || dto.Title.Contains(p.Title) || p.QuestionText.Contains(dto.Title) || dto.Title.Contains(p.QuestionText) || 
					((p.Title.Contains(keys[0]) || p.QuestionText.Contains(keys[0]) || keys[0].Contains(p.Title) || keys[0].Contains(p.QuestionText)) &&
                    (p.Title.Contains(keys[1]) || p.QuestionText.Contains(keys[1]) || keys[1].Contains(p.Title) || keys[1].Contains(p.QuestionText)) &&
                    (p.Title.Contains(keys[2]) || p.QuestionText.Contains(keys[2]) || keys[2].Contains(p.Title) || keys[2].Contains(p.QuestionText))));
                    break;
            }
		}

		if (dto.Category != null)
		{
			var categoryIds = await GetDescendantIds(dto.Category.Value);

			items = items.Where(p => categoryIds.Contains(p.QuestionCategoryId.Value));
		}

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

	private async Task<List<long>> GetDescendantIds(long categoryId)
	{
		var categories = await _context.QuestionCategories.ToListAsync();

		var result = new List<long> { categoryId };

		void AddChildren(long parentId)
		{
			var children = categories
				.Where(x => x.ParentId == parentId)
				.Select(x => x.Id);

			foreach (var child in children)
			{
				result.Add(child);
				AddChildren(child);
			}
		}

		AddChildren(categoryId);

		return result;
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