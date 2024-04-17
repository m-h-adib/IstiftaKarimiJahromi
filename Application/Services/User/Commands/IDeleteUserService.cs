using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.User.Commands
{
	public interface IDeleteUserService
	{
		Task<ResultDto> ExecuteAsync(int id);
	}

	public class DeleteUserService: IDeleteUserService
	{
		#region members
		private readonly IDatabaseContext _context;
		#endregion
		#region ctors
		public DeleteUserService(IDatabaseContext context)
		{
			_context = context;
		}
		#endregion

		public async Task<ResultDto> ExecuteAsync(int id)
		{
			var item = await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

			item.RemovedAt = DateTime.Now;
			item.IsRemoved = true;

			await _context.SaveChangesAsync();

			return ResultDto.Succeeded("ok");
		}
	}
}
