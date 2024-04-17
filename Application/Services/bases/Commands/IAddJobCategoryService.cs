using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IAddJobCategoryService
{
    Task<ResultDto> ExecuteAsync(AddJobCategoryDto dto);
}


public class AddJobCategoryService : IAddJobCategoryService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddJobCategoryService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddJobCategoryDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name == "")
            return ResultDto.Failed("لطفا عنوان دسته شغلی را وارد نمایید");

        if (await _context.JobCategories.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name))
            return ResultDto.Failed("دسته شغلی با این نام موجود می باشد");

        await _context.JobCategories.AddAsync(new Domain.Entities.JobCategory
        {
            IsActive = true,
            IsRemoved = false,
            Order = dto.Order,
            Title = dto.Name,
            CreatedAt = DateTime.Now
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}
#region dtos
public class AddJobCategoryDto
{
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion