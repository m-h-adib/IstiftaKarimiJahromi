using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetOneProvinceService
{
	Task<ResultDto<GetProvinceDto>> ExecuteAsync(int ProvinceId);
}

public class GetOneProvinceService : IGetOneProvinceService
{
	private readonly IDatabaseContext _context;
	public GetOneProvinceService(IDatabaseContext context)
	{
		_context = context;
	}

	public async Task<ResultDto<GetProvinceDto>> ExecuteAsync(int ProvinceId)
	{
		var res = await _context.Provinces.Where(p => !p.IsRemoved && p.Id == ProvinceId)
			.Select(p=>new GetProvinceDto
			{
				Id=p.Id,
				Name=p.Title,
				Order=p.Order,
				CountryId=(int)p.CountryId!
			})
			.FirstOrDefaultAsync();

		return ResultDto<GetProvinceDto>.Succeeded("ok", res);
	}
}