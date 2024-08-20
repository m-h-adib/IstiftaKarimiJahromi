using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Session.Commands;

public interface IDeleteSessionService
{
    Task<ResultDto> ExecuteAsync(long Id);
}

public class DeleteSessionService : IDeleteSessionService
{
    #region members
    private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public DeleteSessionService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion


    public async Task<ResultDto> ExecuteAsync(long Id)
    {
        var res = await _context.Sessions
            .Include(p => p.SessionUsers)
            .FirstOrDefaultAsync(p => p.Id == Id && !p.IsRemoved);

        if (res == null)
        {
            return ResultDto.Failed("جلسه ای یافت نشد");
        }

        res.IsRemoved = true;
        res.RemovedAt = DateTime.Now;
        if (res.SessionUsers != null)
        {
            foreach (var item in res.SessionUsers)
            {
                item.IsRemoved = true;
                item.RemovedAt = DateTime.Now;
            }
        }

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}