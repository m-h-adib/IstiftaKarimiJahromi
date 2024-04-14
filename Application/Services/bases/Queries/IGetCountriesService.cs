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

public interface IGetCountriesService
{
	Task<ResultDto<List<GetCountryDto>>> ExecuteAsync();
}

public class GetCountriesService: IGetCountriesService
{
	private readonly IDatabaseContext _context;
    public GetCountriesService(IDatabaseContext context)
    {
		_context = context;
    }


    public async Task<ResultDto<List<GetCountryDto>>> ExecuteAsync()
	{


		var items = await _context.Countries.Where(p => p.IsActive && !p.IsRemoved).Select(p => new GetCountryDto
		{
			Id=p.Id,
			Name=p.Title,
			Order=p.Order
		}).ToListAsync();

		return ResultDto<List<GetCountryDto>>.Succeeded("ok", items);
	}
}


#region dtos
public class GetCountryDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion