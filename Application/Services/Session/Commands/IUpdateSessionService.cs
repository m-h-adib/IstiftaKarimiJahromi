using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.bases.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Services.Session.Commands.UpdateSessionService;

namespace Application.Services.Session.Commands;

public interface IUpdateSessionService
{
    Task<ResultDto> ExecuteAsync(UpdateSessionDto dto);
}

public class UpdateSessionService : IUpdateSessionService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ISendMessageService _sendMessageService;
    #endregion
    #region ctors
    public UpdateSessionService(IDatabaseContext context,
        ISendMessageService sendMessageService)
    {
        _context = context;
        _sendMessageService = sendMessageService;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(UpdateSessionDto dto)
    {
        var session = await _context.Sessions.Include(p => p.SessionUsers).FirstOrDefaultAsync(p => p.Id == dto.Id);
        if (session == null)
        {
            return ResultDto.Failed("جلسه مورد نظر یافت نشد");
        }

        session.Title = dto.Title;
        session.Time = dto.Time;
        session.Date = dto.Date.ToMiladi();
        session.Description = dto.Description;
        session.UpdatedAt = DateTime.Now;
        if (session.SessionUsers == null)
            session.SessionUsers = new List<SessionUser>();

        if (session.SessionUsers.Any())
        {
            foreach (var item in session.SessionUsers)
            {
                if (dto.Users != null && !dto.Users.Any(p => p == item.UserId))
                {
                    item.IsRemoved = true;
                    item.RemovedAt = DateTime.Now;

                    try
                    {
                        await _sendMessageService.ExecuteAsync(item.UserId!.Value, "با سلام" + Environment.NewLine + "جلسه ای برای شما در تاریخ " + dto.Date + " و ساعت" + dto.Time + " با موضوع " + dto.Title + " برقرار شده است." + Environment.NewLine + "لطفا در زمان تایین شده در جلسه شرکت نمایید." + Environment.NewLine + "با تشکر از همکاری شما." + Environment.NewLine + "http://185.142.157.171:8082/");
                    }
                    catch (Exception ex) { }
                }
            }
        }

        if (dto.Users != null && dto.Users.Any())
        {
            foreach (var item in dto.Users)
            {
                if (!session.SessionUsers.Any(p => p.UserId == item))
                    session.SessionUsers.Add(new SessionUser
                    {
                        UserId = item,
                        SessionId = dto.Id,
                        CreatedAt = DateTime.Now,
                        IsActive = true,
                        IsRemoved = false
                    });
            }
        }

        await _context.SaveChangesAsync();


        return ResultDto.Succeeded("ok");
    }
}

#region dtos
public class UpdateSessionDto
{
    public long? Id { get; set; }
    public string? Title { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public List<long>? Users { get; set; }
    public string? Description { get; set; }
}
#endregion