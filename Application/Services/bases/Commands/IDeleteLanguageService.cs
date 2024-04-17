using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IDeleteLanguageService
{
	Task<ResultDto> ExecuteAsync(long? LanguageId);
}

public class DeleteLanguageService : IDeleteLanguageService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public DeleteLanguageService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(long? LanguageId)
	{
		if (LanguageId == null)
			return ResultDto.Failed("Enter Id");

		var item = await _context.Languages.FirstOrDefaultAsync(p => p.Id == LanguageId && !p.IsRemoved);
		if (item == null)
			return ResultDto.Failed("NOT FOUND");

		item.RemovedAt = DateTime.Now;
		item.IsRemoved = true;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}