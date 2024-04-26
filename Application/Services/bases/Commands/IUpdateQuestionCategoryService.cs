using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IUpdateQuestionCategoryService
{
	Task<ResultDto> ExecuteAsync(UpdateQuestionCategoryDto dto);
}


public class UpdateQuestionCategoryService : IUpdateQuestionCategoryService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateQuestionCategoryService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateQuestionCategoryDto dto)
	{
		if (dto == null || dto.Title == null || dto.Title == "" || dto.Id == null)
			return ResultDto.Failed("لطفا عنوان دسته را وارد نمایید");

		var jobCategory = await _context.QuestionCategories.FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsRemoved);
		if (jobCategory == null)
			return ResultDto.Failed("دسته  با این شماره یافت نشد");
		if (await _context.JobCategories.AnyAsync(p => !p.IsRemoved && p.Title == dto.Title && p.Id != dto.Id))
			return ResultDto.Failed("دسته با این نام موجود می باشد");

		jobCategory.Title = dto.Title;
		jobCategory.Order = dto.Order;
		if (dto.ParentId != null)
			jobCategory.ParentId = dto.ParentId;
		jobCategory.UpdatedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
#region dtos
public class UpdateQuestionCategoryDto
{
	public long? Id { get; set; }
    public string? Title { get; set; }
	public int? Order { get; set; }
    public long? ParentId { get; set; }
}
#endregion