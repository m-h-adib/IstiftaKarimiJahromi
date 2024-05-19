using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Queries;

public interface IGetUsersByRolesService
{
	Task<ResultDto<List<KeyValuePair<long, string>>>> ExecuteAsync(List<int> roles);
}

public class GetUsersByRolesService: IGetUsersByRolesService
{
	private readonly IDatabaseContext _context;
	public GetUsersByRolesService(IDatabaseContext context)
	{
		_context = context;
	}

	public async Task<ResultDto<List<KeyValuePair<long,string>>>> ExecuteAsync(List<int> roles)
	{
		var res = await _context.Users
			.Where(p => !p.IsRemoved && roles.Contains((int)p.Role))
			.Select(p => new KeyValuePair<long, string>(p.Id, p.Name+"("+p.Role.GetDisplayName()+")"))
			.ToListAsync();

		return ResultDto<List<KeyValuePair<long, string>>>.Succeeded("ok", res);
	}
}
