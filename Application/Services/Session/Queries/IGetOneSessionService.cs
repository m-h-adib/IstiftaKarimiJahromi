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

namespace Application.Services.Session.Queries;

public interface IGetOneSessionService
{
    Task<ResultDto<GetOneSessionDto>> ExecuteAsync(long Id);
}

public class GetOneSessionService: IGetOneSessionService
{
    #region members
    private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetOneSessionService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<GetOneSessionDto>> ExecuteAsync(long Id)
    {
        var res = await _context.Sessions
          .Where(p => p.IsActive)
          .Where(p => !p.IsRemoved)
          .Where(p => p.Id == Id)
          .Select(p => new GetOneSessionDto
          {
              Id = p.Id,
              Date = p.Date.Value.ToPersianWithoutTime(),
              Time = p.Time,
              Question = p.Research.QuestionCycle.Question.Title,
              Title = p.Title,
              Users = p.SessionUsers.Select(u => u.User.Id).ToList()
          }).FirstOrDefaultAsync();

        if (res == null)
            return ResultDto<GetOneSessionDto>.Failed("جلسه ای یافت نشد");

        return ResultDto<GetOneSessionDto>.Succeeded("ok", res);
    }
}

public class GetOneSessionDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? Question { get; set; }
    public List<long>? Users { get; set; }
}