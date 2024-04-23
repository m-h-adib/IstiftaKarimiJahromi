using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetOneLanguageService
{
	Task<ResultDto<GetLanguageDto>> ExecuteAsync(int LanguageId);
}

public class GetOneLanguageService : IGetOneLanguageService
{
	private readonly IDatabaseContext _context;
	public GetOneLanguageService(IDatabaseContext context)
	{
		_context = context;
	}

	public async Task<ResultDto<GetLanguageDto>> ExecuteAsync(int LanguageId)
	{
		var res = await _context.Languages.Where(p => !p.IsRemoved && p.Id == LanguageId)
			.Select(p=>new GetLanguageDto
			{
				Id=p.Id,
				Name=p.Title,
				Order=p.Order
			})
			.FirstOrDefaultAsync();

		return ResultDto<GetLanguageDto>.Succeeded("ok", res);
	}
}