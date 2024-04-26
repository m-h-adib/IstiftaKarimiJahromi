using Application.Common;
using Application.Interfaces.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IAddQuestionCategoryService
{
    Task<ResultDto> ExecuteAsync(AddQuestionCategoryDto dto);
}

public class AddQuestionCategoryService: IAddQuestionCategoryService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddQuestionCategoryService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddQuestionCategoryDto dto)
    {
        if (dto.Title == null || dto.Title == "")
            return ResultDto.Failed("وارد کردن عنوان الزامیست");

        await _context.QuestionCategories.AddAsync(new Domain.Entities.QuestionCategory
        {
            Title = dto.Title,
            CreatedAt = DateTime.Now,
            IsActive = true,
            IsRemoved=false,
            Order=dto.Order,
            ParentId=dto.ParentId
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}

#region dtos
public class AddQuestionCategoryDto
{
    public string? Title { get; set; }
    public long? ParentId { get; set; }
    public int? Order { get; set; }
}
#endregion
