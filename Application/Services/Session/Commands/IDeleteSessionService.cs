using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.bases.Commands;
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
    private readonly ISendMessageService _sendMessageService;
    #endregion
    #region ctors
    public DeleteSessionService(IDatabaseContext context,
        ISendMessageService sendMessageService)
    {
        _context = context;
        _sendMessageService = sendMessageService;
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

                await _sendMessageService.ExecuteAsync(item.UserId!.Value, "با سلام" + Environment.NewLine + "جلسه شما در تاریخ " + res.Date!.Value.ToPersianWithoutTime() + " و ساعت" + res.Time + " با موضوع " + res.Title + " لغو شد." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
            }
        }

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}