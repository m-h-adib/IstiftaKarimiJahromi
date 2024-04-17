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

public interface IGetMethodOfRecieveQuestionsService
{
	Task<ResultDto<List<GetMethodOfRecieveQuestionDto>>> ExecuteAsync();
}

public class GetMethodOfRecieveQuestionsService : IGetMethodOfRecieveQuestionsService
{
	private readonly IDatabaseContext _context;
    public GetMethodOfRecieveQuestionsService(IDatabaseContext context)
    {
		_context = context;
    }


    public async Task<ResultDto<List<GetMethodOfRecieveQuestionDto>>> ExecuteAsync()
	{


		var items = await _context.Countries.Where(p => p.IsActive && !p.IsRemoved).OrderBy(p => p.Order).Select(p => new GetMethodOfRecieveQuestionDto
		{
			Id=p.Id,
			Name=p.Title,
			Order=p.Order
		}).ToListAsync();

		return ResultDto<List<GetMethodOfRecieveQuestionDto>>.Succeeded("ok", items);
	}
}


#region dtos
public class GetMethodOfRecieveQuestionDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion