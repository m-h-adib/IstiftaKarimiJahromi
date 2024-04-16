using Application.Services.Authentication.Methods;
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

public interface IGetMenuAccessService
{
	Task<ResultDto<List<GetMenuAccessDto>>> ExecuteAsync();
}

public class GetMenuAccessService : IGetMenuAccessService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public GetMenuAccessService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
	}
	#endregion

	public async Task<ResultDto<List<GetMenuAccessDto>>> ExecuteAsync()
	{
		var userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

		var access = await _context.AdminSectionAccesses.Where(p => p.UserId == userid && !p.IsRemoved && p.IsActive).Select(p => p.AdminSectionId).ToListAsync();

		var res = await _context.AdminSections.Where(p => p.ParentId == null && (access.Contains(p.Id) && p.ShowInMenu==true || (p.IsPublic == true && p.ShowInMenu==true))).OrderBy(p => p.Order)
			.Select(p => new GetMenuAccessDto
			{
				Id = p.Id,
				Title = p.Title,
				Name = p.Name,
				Link = p.Link,
				QueryParams = p.QueryParams,
				Childs = _context.AdminSections.Where(u => u.ParentId == p.Id  && (access.Contains(u.Id) || u.IsPublic == true) && u.ShowInMenu==true).OrderBy(u=>u.Order).Select(u => new GetMenuAccessDto
				{
					Id = u.Id,
					Name = u.Name,
					Title = u.Title,
					Link = u.Link,
					QueryParams = u.QueryParams,
					Childs = _context.AdminSections.Where(t => t.ParentId == u.Id && (access.Contains(t.Id) || t.IsPublic == true) && t.ShowInMenu == true).OrderBy(t=>t.Order).Select(t => new GetMenuAccessDto
					{
						Id = t.Id,
						Name = t.Name,
						Title = t.Title,
						Link = t.Link,
						QueryParams = t.QueryParams,
						Childs = null
					}).ToList()
				}).ToList()
			}).ToListAsync();

		return ResultDto<List<GetMenuAccessDto>>.Succeeded("ok", res);
	}
}

#region dtos
public class GetMenuAccessDto
{
	public long? Id { get; set; }
	public string? Name { get; set; }
	public string? Title { get; set; }
	public string? Link { get; set; }
	public string? QueryParams { get; set; }
	public List<GetMenuAccessDto>? Childs { get; set; }
}
#endregion