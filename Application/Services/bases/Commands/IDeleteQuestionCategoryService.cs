using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IDeleteQuestionCategoryService
{
	Task<ResultDto> ExecuteAsync(long? QuestionCategoryId);
}

public class DeleteQuestionCategoryService: IDeleteQuestionCategoryService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public DeleteQuestionCategoryService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(long? QuestionCategoryId)
	{
		if (QuestionCategoryId == null)
			return ResultDto.Failed("Enter Id");

		var item = await _context.QuestionCategories.FirstOrDefaultAsync(p => p.Id == QuestionCategoryId && !p.IsRemoved);
		if (item == null)
			return ResultDto.Failed("NOT FOUND");

		item.RemovedAt = DateTime.Now;
		item.IsRemoved = true;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
