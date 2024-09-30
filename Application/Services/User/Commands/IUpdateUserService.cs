using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User.Commands;

public interface IUpdateUserService
{
    Task<ResultDto> ExecuteAsync(UpdateUserDto dto);
}

public class UpdateUserService: IUpdateUserService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public UpdateUserService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _tokenContents = tokenContents;
    }
    #endregion


    public async Task<ResultDto> ExecuteAsync(UpdateUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == dto.UserId);
        if (user != null)
        {
            user.Name = dto.Name;
            user.Username = dto.Username;
            user.Role = (RoleType)dto.Role!.Value;

            await _context.SaveChangesAsync();

            return ResultDto.Succeeded("ثبت با موفقیت انجام شد", 200);
        }

        return ResultDto.Failed("کاربری یافت نشد", 404);
    }
}

#region dtos
public class UpdateUserDto
{
    public long? UserId { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public int? Role { get; set; }
}
#endregion