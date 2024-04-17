using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IAddProvinceService
{
    Task<ResultDto> ExecuteAsync(AddProvinceDto dto);
}


public class AddProvinceService : IAddProvinceService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public AddProvinceService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddProvinceDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name == "" || dto.CountryId==null)
            return ResultDto.Failed("لطفا نام کشور را وارد نمایید");

        if (await _context.Provinces.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name && p.CountryId==dto.CountryId))
            return ResultDto.Failed("استانی با این نام موجود می باشد");

        await _context.Provinces.AddAsync(new Domain.Entities.Province
        {
            IsActive = true,
            IsRemoved = false,
            Order = dto.Order,
            Title = dto.Name,
            CreatedAt = DateTime.Now,
            CountryId = dto.CountryId
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}
#region dtos
public class AddProvinceDto
{
    public long? CountryId { get; set; }
    public string? Name { get; set; }
    public int? Order { get; set; }
}
#endregion