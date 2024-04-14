using Application.Services.Authentication.Methods;
using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication.Queries;

public interface IGetOneAccessService
{
    Task<ResultDto> ExecuteAsync(int SectionId);
}

public class GetOneAccessService: IGetOneAccessService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetOneAccessService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(int SectionId)
    {
        var userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        var res = await _context.AdminSectionAccesses.AnyAsync(p => p.IsActive && !p.IsRemoved && p.UserId == userid && p.AdminSectionId == SectionId);
        if(res)
        {
            return ResultDto.Succeeded("ok");
        }
        else
        {
            return ResultDto.Failed("Not Access");
        }
    }
}
