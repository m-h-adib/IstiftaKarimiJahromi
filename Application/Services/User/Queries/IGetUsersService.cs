using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Authentication.Queries;
using Domain.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AdminServices.User.Queries;

public interface IGetUsersService
{
	Task<ResultDto<ResultOfList<GetAdminsResponeDto>>> ExecuteAsync(GetAminsRequestDto dto);
}

public class GetUsersService: IGetUsersService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IGetOneAccessService _getOneAccessService;
    #endregion
    #region ctors
    public GetUsersService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor,
		IGetOneAccessService getOneAccessService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _tokenContents = tokenContents;
		_getOneAccessService = getOneAccessService;
    }
    #endregion

    public async Task<ResultDto<ResultOfList<GetAdminsResponeDto>>> ExecuteAsync(GetAminsRequestDto dto)
    {
		var userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

		if (!await _context.Users.AnyAsync(p => p.Id == userid && p.Role == RoleType.Admin))
		{
			return ResultDto<ResultOfList<GetAdminsResponeDto>>.Failed("شما دسترسی لازم به این صفحه را ندارید");
		}


		var items = _context.Users.Where(p => !p.IsRemoved);
		
		if (dto.Username != null && dto.Username!="")
			items = items.Where(p => p.Username == dto.Username);
		if (dto.Name != null && dto.Name != "")
			items = items.Where(p => (p.Name + " " + p.Name).Contains(dto.Name));

		PasswordHasher hasher = new PasswordHasher();

		int rows = 0;
		var res = await items.Select(p => new GetAdminsResponeDto
		{
			Id = p.Id,
			Name=p.Name,
			Username=p.Username
		}).ToPaged(dto.PageNember, dto.PageSize, out rows).ToListAsync();

		var result = new ResultOfList<GetAdminsResponeDto>
		{
			Items = res,
			Paging = new(dto.PageNember, dto.PageSize, rows)
		};

		return ResultDto<ResultOfList<GetAdminsResponeDto>>.Succeeded("ok", result);
	}
}

#region dtos
public class GetAminsRequestDto
{
	public int PageNember { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	public string? Username { get; set; }
	public string? Name { get; set; }
}

public class GetAdminsResponeDto
{
	public long Id { get; set; }
	public string? Name { get; set; }
	public string? Username { get; set; }
}
#endregion
