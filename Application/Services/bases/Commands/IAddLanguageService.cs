using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IAddLanguageService
{
    Task<ResultDto> ExecuteAsync(AddLanguageDto dto);
}


public class AddLanguageService : IAddLanguageService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddLanguageService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddLanguageDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name == "")
            return ResultDto.Failed("لطفا زبان را وارد نمایید");

        if (await _context.Languages.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name))
            return ResultDto.Failed("زبانی با این نام موجود می باشد");

        await _context.Languages.AddAsync(new Domain.Entities.Language
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
public class AddLanguageDto
{
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion