using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IAddCountryService
{
    Task<ResultDto> ExecuteAsync(AddCountryDto dto);
}


public class AddCountryService: IAddCountryService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddCountryService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddCountryDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name == "")
            return ResultDto.Failed("لطفا نام کشور را وارد نمایید");

        if (await _context.Countries.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name))
            return ResultDto.Failed("کشوری با این نام موجود می باشد");

        await _context.Countries.AddAsync(new Domain.Entities.Country
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
public class AddCountryDto
{
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion