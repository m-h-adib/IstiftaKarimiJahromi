using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IUpdateProvinceService
{
	Task<ResultDto> ExecuteAsync(UpdateProvinceDto dto);
}


public class UpdateProvinceService : IUpdateProvinceService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateProvinceService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateProvinceDto dto)
	{
		if (dto == null || dto.Name == null || dto.Name == "" || dto.Id == null)
			return ResultDto.Failed("لطفا نام کشور را وارد نمایید");

		var province = await _context.Provinces.FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsRemoved);
		if (province == null)
			return ResultDto.Failed("استانی با این شماره یافت نشد");
		if (await _context.Provinces.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name && p.CountryId==dto.CountryId && p.Id != dto.Id))
			return ResultDto.Failed("استانی با این نام موجود می باشد");

		province.Title = dto.Name;
		province.Order = dto.Order;
		province.CountryId = dto.CountryId;
		province.UpdatedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
#region dtos
public class UpdateProvinceDto
{
	public long? Id { get; set; }
    public long? CountryId { get; set; }
    public string? Name { get; set; }
	public int? Order { get; set; }
}
#endregion