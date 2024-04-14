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

public interface IGetProvincesService
{
	Task<ResultDto<List<GetProvinceDto>>> ExecuteAsync(long? CountryId);
}

public class GetProvincesService: IGetProvincesService
{
	private readonly IDatabaseContext _context;
    public GetProvincesService(IDatabaseContext context)
    {
		_context = context;
    }


    public async Task<ResultDto<List<GetProvinceDto>>> ExecuteAsync(long? CountryId)
	{
		if (CountryId == null)
			return ResultDto<List<GetProvinceDto>>.Failed("لطفا شماره کشور را وارد نمایید", 400);

		var items=await _context.Provinces.Where(p => p.IsActive && !p.IsRemoved && p.CountryId==CountryId).Select(p => new GetProvinceDto
		{
			Id = p.Id,
			Name = p.Title,
			Order = p.Order
		}).ToListAsync();

		return ResultDto<List<GetProvinceDto>>.Succeeded("ok", items);
	}
}


#region dtos
public class GetProvinceDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
	public int? Order { get; set; }
}
#endregion