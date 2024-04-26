using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetQuestionCategoriesService
{
	Task<ResultDto<List<GetQuestionCategoryDto>>> ExecuteAsync();
}

public class GetQuestionCategoriesService: IGetQuestionCategoriesService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetQuestionCategoriesService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<List<GetQuestionCategoryDto>>> ExecuteAsync()
    {
		List<GetQuestionCategoryDto> tree = new List<GetQuestionCategoryDto>();

		List<QuestionCategory> items = await _context.QuestionCategories.Where(p => !p.IsRemoved).ToListAsync();

		List<QuestionCategory> rootItems = items.Where(p => p.ParentId == null && !p.IsRemoved).OrderBy(p => p.Order).ToList();

		foreach (QuestionCategory item in rootItems)
		{
			GetQuestionCategoryDto tvi = new GetQuestionCategoryDto()
			{
				Id=item.Id,
				Title=item.Title,
				Order=item.Order
			};
			
			BuildChildNodes(tvi, items, item.Id);
			tree.Add(tvi);
		}

		return ResultDto<List<GetQuestionCategoryDto>>.Succeeded("ok", tree);
	}

	private void BuildChildNodes(GetQuestionCategoryDto parentNode, List<QuestionCategory> items, long parentID)
	{
		List<QuestionCategory> children = items.Where(p => p.ParentId == parentID && !p.IsRemoved).OrderBy(p => p.Order).ToList();
		
		parentNode.Childs = new List<GetQuestionCategoryDto>();
		foreach (QuestionCategory item in children)
		{
			GetQuestionCategoryDto tvi = new GetQuestionCategoryDto()
			{
				Id=item.Id,
				Order=item.Order,
				Title=item.Title
			};
			
			parentNode.Childs.Add(tvi);
			BuildChildNodes(tvi, items, item.Id);
		}
	}
}

#region dtos
public class GetQuestionCategoryDto 
{
    public long? Id { get; set; }
	public string? Title { get; set; }
	public int? Order { get; set; }
    public ICollection<GetQuestionCategoryDto>? Childs { get; set; }
}
#endregion
