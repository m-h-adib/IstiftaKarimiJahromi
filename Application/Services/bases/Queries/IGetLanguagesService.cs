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

public interface IGetLanguagesService
{
	Task<ResultDto<List<GetLanguageDto>>> ExecuteAsync();
}

public class GetLanguagesService : IGetLanguagesService
{
	private readonly IDatabaseContext _context;
    public GetLanguagesService(IDatabaseContext context)
    {
		_context = context;
    }


    public async Task<ResultDto<List<GetLanguageDto>>> ExecuteAsync()
	{


		var items = await _context.Languages.Where(p => p.IsActive && !p.IsRemoved).OrderBy(p => p.Order).Select(p => new GetLanguageDto
		{
			Id=p.Id,
			Name=p.Title,
			Order=p.Order
		}).ToListAsync();

		return ResultDto<List<GetLanguageDto>>.Succeeded("ok", items);
	}
}


#region dtos
public class GetLanguageDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion