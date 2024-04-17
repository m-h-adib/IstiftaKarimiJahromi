using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IAddMethodOfReceiveQuestionService
{
    Task<ResultDto> ExecuteAsync(AddMethodOfReceiveQuestionDto dto);
}


public class AddMethodOfReceiveQuestionService : IAddMethodOfReceiveQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddMethodOfReceiveQuestionService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddMethodOfReceiveQuestionDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name == "")
            return ResultDto.Failed("لطفا نحوه دریافت سوال را وارد نمایید");

        if (await _context.MethodOfReceiveQuestions.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name))
            return ResultDto.Failed("نحوه دریافت سوال با این عنوان موجود می باشد");

        await _context.MethodOfReceiveQuestions.AddAsync(new Domain.Entities.MethodOfReceiveQuestion
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
public class AddMethodOfReceiveQuestionDto
{
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion