using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetOneCountryService
{
	Task<ResultDto<GetCountryDto>> ExecuteAsync(int CountryId);
}

public class GetOneCountryService: IGetOneCountryService
{
	private readonly IDatabaseContext _context;
	public GetOneCountryService(IDatabaseContext context)
	{
		_context = context;
	}

	public async Task<ResultDto<GetCountryDto>> ExecuteAsync(int CountryId)
	{
		var res = await _context.Countries.Where(p => !p.IsRemoved && p.Id == CountryId)
			.Select(p=>new GetCountryDto
			{
				Id=p.Id,
				Name=p.Title,
				Order=p.Order
			})
			.FirstOrDefaultAsync();

		return ResultDto<GetCountryDto>.Succeeded("ok", res);
	}
}