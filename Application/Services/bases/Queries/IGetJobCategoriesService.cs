using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetJobCategoriesService
{
	Task<ResultDto<List<GetJobCategoryDto>>> ExecuteAsync();
}

public class GetJobCategoriesService : IGetJobCategoriesService
{
	private readonly IDatabaseContext _context;
    public GetJobCategoriesService(IDatabaseContext context)
    {
		_context = context;
    }


    public async Task<ResultDto<List<GetJobCategoryDto>>> ExecuteAsync()
	{


		var items = await _context.JobCategories.Where(p => p.IsActive && !p.IsRemoved).Select(p => new GetJobCategoryDto
		{
			Id=p.Id,
			Name=p.Title,
			Order=p.Order
		}).ToListAsync();

		return ResultDto<List<GetJobCategoryDto>>.Succeeded("ok", items);
	}
}


#region dtos
public class GetJobCategoryDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion