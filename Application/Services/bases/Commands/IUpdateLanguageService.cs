using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IUpdateLanguageService
{
	Task<ResultDto> ExecuteAsync(UpdateLanguageDto dto);
}


public class UpdateLanguageService : IUpdateLanguageService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateLanguageService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateLanguageDto dto)
	{
		if (dto == null || dto.Name == null || dto.Name == "" || dto.Id == null)
			return ResultDto.Failed("لطفا زبان را وارد نمایید");

		var language = await _context.Languages.FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsRemoved);
		if (language == null)
			return ResultDto.Failed("زبانی با این شماره یافت نشد");
		if (await _context.Languages.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name && p.Id != dto.Id))
			return ResultDto.Failed("زبان با این نام موجود می باشد");

		language.Title = dto.Name;
		language.Order = dto.Order;
		language.UpdatedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
#region dtos
public class UpdateLanguageDto
{
	public long? Id { get; set; }
	public string? Name { get; set; }
	public int? Order { get; set; }
}
#endregion