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



    public async Task<ResultDto<List<SearchQuestionDto>>> ExecuteAsync(
        SearchQuestionRequestDto dto)
    {
        var items = _context.Questions
            .AsNoTracking()
            .Where(p =>
                !p.IsRemoved &&
                p.DuplicateId == null &&
                p.Step == Domain.Enums.QuestionStepType.Archive);

        // Search by title and question text
        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            var keys = dto.Title
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToList();

            foreach (var key in keys)
            {
                var keyword = key;

                items = items.Where(p =>
                    (p.Title != null && p.Title.Contains(keyword)) ||
                    (p.QuestionText != null && p.QuestionText.Contains(keyword)));
            }
        }

        // Filter by category and its descendants
        if (dto.Category.HasValue)
        {
            var categoryIds = await GetDescendantIds(dto.Category.Value);

            categoryIds.Add(dto.Category.Value);

            items = items.Where(p =>
                p.QuestionCategoryId.HasValue &&
                categoryIds.Contains(p.QuestionCategoryId.Value));
        }

        // Execute query
        var res = await items
            .Select(p => new SearchQuestionDto
            {
                Id = p.Id,
                Title = p.Title,
                Text = p.QuestionText,
                Category = p.QuestionCategoryId.HasValue
                    ? p.QuestionCategory!.Title
                    : null
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