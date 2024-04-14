using Application.Services.Authentication.Methods;
using Application.Services.Authentication.Queries;
using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Authentication.Queries;

public interface IGetAccessService
{
	Task<ResultDto<List<long>>> ExecuteAsync(int role);
}

public class GetAccessService: IGetAccessService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public GetAccessService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
	}
	#endregion

	public async Task<ResultDto<List<long>>> ExecuteAsync(int role)
	{
		var userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

		var access = await _context.AdminSectionAccesses.Where(p => p.UserId == userid && p.AdminSection!.Role==(RoleType)role && !p.IsRemoved && p.IsActive).Select(p => p.AdminSectionId).ToListAsync();

		return ResultDto<List<long>>.Succeeded("ok", access);
	}
}
