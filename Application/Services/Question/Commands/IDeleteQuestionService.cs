using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Question.Commands;

public interface IDeleteQuestionService
{
	Task<ResultDto> ExecuteAsync(long? QuestionId);
}

public class DeleteQuestionService: IDeleteQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public DeleteQuestionService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion


	public async Task<ResultDto> ExecuteAsync(long? QuestionId)
	{
		if (QuestionId == null)
			return ResultDto.Failed("Bad Request", 400);

		var res= await _context.Questions
			.Where(p => p.Id == QuestionId && !p.IsRemoved)
			.FirstOrDefaultAsync();

		if (res == null)
			return ResultDto.Failed("سوالی یافت نشد", 404);

		res.IsRemoved = true;
		res.RemovedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}