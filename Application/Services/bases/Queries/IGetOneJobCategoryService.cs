using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetOneJobCategoryService
{
	Task<ResultDto<GetJobCategoryDto>> ExecuteAsync(int JobCategoryId);
}

public class GetOneJobCategoryService : IGetOneJobCategoryService
{
	private readonly IDatabaseContext _context;
	public GetOneJobCategoryService(IDatabaseContext context)
	{
		_context = context;
	}

	public async Task<ResultDto<GetJobCategoryDto>> ExecuteAsync(int JobCategoryId)
	{
		var res = await _context.JobCategories.Where(p => !p.IsRemoved && p.Id == JobCategoryId)
			.Select(p=>new GetJobCategoryDto
			{
				Id=p.Id,
				Name=p.Title,
				Order=p.Order
			})
			.FirstOrDefaultAsync();

		return ResultDto<GetJobCategoryDto>.Succeeded("ok", res);
	}
}