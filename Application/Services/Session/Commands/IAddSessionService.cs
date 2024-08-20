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

namespace Application.Services.Session.Commands;

public interface IAddSessionService
{
    Task<ResultDto> ExecuteAsync(AddSessionDto dto);
}

public class AddSessionService: IAddSessionService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _contextAccessor;
    #endregion
    #region ctors
    public AddSessionService(IDatabaseContext context,
        IHttpContextAccessor contextAccessor,
        ITokenContents tokenContents)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _tokenContents = tokenContents;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddSessionDto dto)
    {
        var userid = _tokenContents.GetUserId(_contextAccessor.HttpContext!.User);
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userid);

        var res = await _context.Sessions.AddAsync(new Domain.Entities.Session
        {
            IsActive = true,
            IsRemoved = false,
            CreatedAt = DateTime.Now,
            Date = dto.Date.ToMiladi(),
            ResearchId = dto.ResearchId,
            Time = dto.Time,
            Title = dto.Title,
            UserId = userid,
            SessionUsers = dto.Users.Select(u => new Domain.Entities.SessionUser
            {
                IsActive = true,
                IsRemoved = false,
                UserId = u,
                CreatedAt = DateTime.Now
            }).ToList()
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("ok");
    }
}
#region dtos
public class AddSessionDto
{
    public string? Title { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public long? ResearchId { get; set; }
    public List<long>? Users { get; set; }
}
#endregion
