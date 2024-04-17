using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IUpdateCountryService
{
	Task<ResultDto> ExecuteAsync(UpdateCountryDto dto);
}


public class UpdateCountryService : IUpdateCountryService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateCountryService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateCountryDto dto)
	{
		if (dto == null || dto.Name == null || dto.Name == "" || dto.Id == null)
			return ResultDto.Failed("لطفا نام کشور را وارد نمایید");

		var country = await _context.Countries.FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsRemoved);
		if (country == null)
			return ResultDto.Failed("کشوری با این شماره یافت نشد");
		if (await _context.Countries.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name && p.Id != dto.Id))
			return ResultDto.Failed("کشوری با این نام موجود می باشد");

		country.Title = dto.Name;
		country.Order = dto.Order;
		country.UpdatedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
#region dtos
public class UpdateCountryDto
{
	public long? Id { get; set; }
	public string? Name { get; set; }
	public int? Order { get; set; }
}
#endregion