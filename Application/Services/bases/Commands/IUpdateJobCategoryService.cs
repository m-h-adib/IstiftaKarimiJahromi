using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IUpdateJobCategoryService
{
	Task<ResultDto> ExecuteAsync(UpdateJobCategoryDto dto);
}


public class UpdateJobCategoryService : IUpdateJobCategoryService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateJobCategoryService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateJobCategoryDto dto)
	{
		if (dto == null || dto.Name == null || dto.Name == "" || dto.Id == null)
			return ResultDto.Failed("لطفا عنوان دسته شغلی را وارد نمایید");

		var jobCategory = await _context.JobCategories.FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsRemoved);
		if (jobCategory == null)
			return ResultDto.Failed("دسته شغلی با این شماره یافت نشد");
		if (await _context.JobCategories.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name && p.Id != dto.Id))
			return ResultDto.Failed("دسته شغلی با این نام موجود می باشد");

		jobCategory.Title = dto.Name;
		jobCategory.Order = dto.Order;
		jobCategory.UpdatedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
#region dtos
public class UpdateJobCategoryDto
{
	public long? Id { get; set; }
    public string? Name { get; set; }
	public int? Order { get; set; }
}
#endregion