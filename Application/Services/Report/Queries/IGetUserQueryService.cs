using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Report.Queries;

public interface IGetUserQueryService
{
    Task<ResultDto<ResultOfList<GetUserQueryResponseDto>>> ExecuteAsync(GetUserQueryRequestDto dto);
}

public class GetUserQueryService: IGetUserQueryService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetUserQueryService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<ResultOfList<GetUserQueryResponseDto>>> ExecuteAsync(GetUserQueryRequestDto dto)
    {
        if (dto.UserId == null)
            return ResultDto<ResultOfList<GetUserQueryResponseDto>>.Failed("لطفا کاربر را انتخاب نمایید");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == dto.UserId);

        if (user == null)
            return ResultDto<ResultOfList<GetUserQueryResponseDto>>.Failed("کاربر یافت نشد", 404);

        var items = _context.Questions
            .Where(p => !p.IsRemoved)
            .Where(p=>p.QuestionCycles.Any(u=>u.ToUserId==dto.UserId));

        if (!string.IsNullOrEmpty(dto.FromDate))
            items = items.Where(p => p.QuestionCycles.Any(u => u.CreatedAt >= dto.FromDate.ToMiladi()));

        if (!string.IsNullOrEmpty(dto.ToDate))
            items = items.Where(p => p.QuestionCycles.Any(u => u.CreatedAt <= dto.ToDate.ToMiladi()));

        if(dto.Type!=null)
        {
            if (dto.Type == 1)
                items = items.Where(p => p.QuestionCycles.Any(u => u.IsAccept == true));
            else if (dto.Type == 2)
                items = items.Where(p => p.QuestionCycles.Any(u => u.PreProccess != null || u.Editors != null || u.Research != null));  
        }

        var res = await items
            .OrderByDescending(p => p.CreatedAt)
            .ToPaged(dto.Page,dto.Size,out int rows)
            .Select(p => new GetUserQueryResponseDto
            {
                Id = p.Id,
                Category = p.QuestionCategory.Title,
                Question = p.Title,
                Date = p.CreatedAt.ToPersianWithoutTime()
            })
            .ToListAsync();

        var result= new ResultOfList<GetUserQueryResponseDto>
        {
            Items = res,
            Paging = new(dto.Page, dto.Size, rows)
        };

        return ResultDto<ResultOfList<GetUserQueryResponseDto>>.Succeeded("ok", result);
    }
}
#region dtos
public class GetUserQueryRequestDto
{
    public long? UserId { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public int? Type { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 20;
}
public class GetUserQueryResponseDto
{
    public long? Id { get; set; }
    public string? Question { get; set; }
    public string? Date { get; set; }
    public string? Category { get; set; }
}
#endregion