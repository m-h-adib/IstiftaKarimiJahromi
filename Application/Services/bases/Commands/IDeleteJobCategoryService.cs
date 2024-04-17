using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IDeleteJobCategoryService
{
	Task<ResultDto> ExecuteAsync(long? JobCategoryId);
}

public class DeleteJobCategoryService : IDeleteJobCategoryService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public DeleteJobCategoryService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(long? JobCategoryId)
	{
		if (JobCategoryId == null)
			return ResultDto.Failed("Enter Id");

		var item = await _context.JobCategories.FirstOrDefaultAsync(p => p.Id == JobCategoryId && !p.IsRemoved);
		if (item == null)
			return ResultDto.Failed("NOT FOUND");

		item.RemovedAt = DateTime.Now;
		item.IsRemoved = true;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}