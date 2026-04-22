using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User.Queries;

public interface IGetOneUserService
{
    Task<ResultDto<GetOneUserDto>> ExecuteAsync(long? UserId);
}

public class GetOneUserService: IGetOneUserService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetOneUserService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _tokenContents = tokenContents;
    }
    #endregion

    public async Task<ResultDto<GetOneUserDto>> ExecuteAsync(long? UserId)
    {
        var res = await _context.Users
            .Where(p => p.Id == UserId)
            .Select(p => new GetOneUserDto
            {
                UserId = p.Id,
                Name = p.Name,
                Username = p.Username,
                Role = (int)p.Role!.Value,
                EitaaId=p.EitaaId
            }).FirstOrDefaultAsync();

        return ResultDto<GetOneUserDto>.Succeeded("ok",res);
    }

}
#region dtos
public class GetOneUserDto
{
    public long? UserId { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public int? Role { get; set; }
    public string? EitaaId { get; set; }
}
#endregion