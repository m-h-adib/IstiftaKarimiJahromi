using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User.Commands;

public interface IUpdateUserAccessService
{
	Task<ResultDto> ExecuteAsync(UpdateUserAccessDto dto);
}

public class UpdateUserAccessService: IUpdateUserAccessService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateUserAccessService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateUserAccessDto dto)
	{
		var access = await _context.AdminSectionAccesses.Where(p => p.UserId == dto.UserId && p.IsActive && !p.IsRemoved).ToListAsync();

		foreach (var item in access)
		{
			if (!dto.Access.Contains(item.AdminSectionId))
			{
				item.IsRemoved = true;
				item.RemovedAt = DateTime.Now;
			}
		}

		foreach (var item in dto.Access)
		{
			if(!access.Any(p=>p.AdminSectionId==item))
			{
				await _context.AdminSectionAccesses.AddAsync(new Domain.Entities.AdminSectionAccess
				{
					AdminSectionId = item,
					UserId = dto.UserId!.Value
				});
			}
		}

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}


#region dtos
public class UpdateUserAccessDto
{
    public long? UserId { get; set; }
    public List<long> Access { get; set; }
}
#endregion