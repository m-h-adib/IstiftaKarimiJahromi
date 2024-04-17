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

namespace Application.AdminServices.User.Queries
{
	public interface IGetUserAccessService
	{
		Task<ResultDto<List<GetUserAccessDto>>> ExecuteAsync(int id);
	}

	public class GetUserAccessService : IGetUserAccessService
	{
		#region members
		private readonly IDatabaseContext _context;
		private readonly ITokenContents _tokenContents;
		private readonly IHttpContextAccessor _httpContextAccessor;
		#endregion
		#region ctors
		public GetUserAccessService(IDatabaseContext context,
			ITokenContents tokenContents,
			IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_tokenContents = tokenContents;
		}
		#endregion

		public async Task<ResultDto<List<GetUserAccessDto>>> ExecuteAsync(int id)
		{
			//var userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

			var access = await _context.AdminSectionAccesses.Where(p => p.UserId == id && !p.IsRemoved && p.IsActive).Select(p => p.AdminSectionId).ToListAsync();

			var res = await _context.AdminSections.Where(p => p.ParentId == null && p.IsPublic==false).OrderBy(p => p.Id)
				.Select(p => new GetUserAccessDto
				{
					Id = p.Id,
					Title = p.Title,
					Name = p.Name,
					IsAccess=access.Any(u=>u==p.Id),
					Childs = _context.AdminSections.Where(u => u.ParentId == p.Id && u.IsPublic==false).OrderBy(u=>u.Order).Select(u => new GetUserAccessDto
					{
						Id = u.Id,
						Name = u.Name,
						Title = u.Title,
						IsAccess = access.Any(t => t == u.Id),
						Childs = _context.AdminSections.Where(t => t.ParentId == u.Id && t.IsPublic == false).OrderBy(t=>t.Order).Select(t => new GetUserAccessDto
						{
							Id = t.Id,
							Name = t.Name,
							Title = t.Title,
							IsAccess = access.Any(y => y == t.Id),
							Childs = null
						}).ToList()
					}).ToList()
				}).ToListAsync();

			return ResultDto<List<GetUserAccessDto>>.Succeeded("ok", res);
		}
	}
}

#region dtos
public class GetUserAccessDto
{
	public long? Id { get; set; }
	public string? Name { get; set; }
	public string? Title { get; set; }
    public bool? IsAccess { get; set; }
    public List<GetUserAccessDto>? Childs { get; set; }
}
#endregion