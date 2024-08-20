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

public interface IGetSessionsService
{
    Task<ResultDto<ResultOfList<GetSessionDto>>> ExecuteAsync(GetSessionRequestDto dto);
}


public class GetSessionsService: IGetSessionsService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetSessionsService(IHttpContextAccessor httpContextAccessor,
        ITokenContents tokenContents,
        IDatabaseContext context)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    public async Task<ResultDto<ResultOfList<GetSessionDto>>> ExecuteAsync(GetSessionRequestDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userid);

        var items = _context.Sessions
            .Where(p => p.IsActive)
            .Where(p => !p.IsRemoved);

        if (user!.Role != Domain.Enums.RoleType.Admin)
        {
            items = items.Where(p => p.SessionUsers.Any(u => u.UserId == userid));
        }

        var res = await items
            .OrderByDescending(p => p.CreatedAt)
            .ToPaged(dto.PageNumber,dto.PageSize,out int rows)
            .Select(p => new GetSessionDto
            {
                Id = p.Id,
                Title = p.Title,
                Date = p.Date.Value.ToPersianWithoutTime(),
                Time = p.Time,
                Question = p.Research.QuestionCycle.Question.Title,
                Users = p.SessionUsers.Select(u => u.User.Name).ToList()
            })
            .ToListAsync();

        var result = new ResultOfList<GetSessionDto>
        {
            Items = res,
            Paging = new(dto.PageNumber, dto.PageSize, rows)
        };

        return ResultDto<ResultOfList<GetSessionDto>>.Succeeded("ok", result);
    }
}

#region dtos
public class GetSessionRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class GetSessionDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? Question { get; set; }
    public List<string>? Users { get; set; }
}
#endregion