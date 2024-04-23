using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetOneMethodOfRecieveQuestionService
{
	Task<ResultDto<GetMethodOfRecieveQuestionDto>> ExecuteAsync(int MethodId);
}

public class GetOneMethodOfRecieveQuestionService : IGetOneMethodOfRecieveQuestionService
{
	private readonly IDatabaseContext _context;
	public GetOneMethodOfRecieveQuestionService(IDatabaseContext context)
	{
		_context = context;
	}

	public async Task<ResultDto<GetMethodOfRecieveQuestionDto>> ExecuteAsync(int MethodId)
	{
		var res = await _context.MethodOfReceiveQuestions.Where(p => !p.IsRemoved && p.Id == MethodId)
			.Select(p=>new GetMethodOfRecieveQuestionDto
			{
				Id=p.Id,
				Name=p.Title,
				Order=p.Order
			})
			.FirstOrDefaultAsync();

		return ResultDto<GetMethodOfRecieveQuestionDto>.Succeeded("ok", res);
	}
}