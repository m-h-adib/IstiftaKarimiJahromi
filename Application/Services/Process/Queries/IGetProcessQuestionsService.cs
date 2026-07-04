using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Process.Quesries;

public interface IGetProcessQuestionsService
{
    Task<ResultDto<ResultOfList<GetProcessQuestionDto>>> ExecuteAsync(GetProcessQuestionRequestDto dto);
}

public class GetProcessQuestionsService : IGetProcessQuestionsService
{
	#region members
	private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetProcessQuestionsService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion

    public async Task<ResultDto<ResultOfList<GetProcessQuestionDto>>> ExecuteAsync(GetProcessQuestionRequestDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);
        var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userid);



        var items = _context.Questions
            .Where(p => !p.IsRemoved)
            .Where(p => p.Step == QuestionStepType.Processing);
            //.Where(p => user!.Role == RoleType.Admin ? true : p.QuestionCycles.Any(u=>u.ToUserId==userid && u.ToStep==QuestionStepType.Processing));

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "ShowAllProcessQuestion"))
            items = items.Where(p => p.QuestionCycles.Any(u => u.ToUserId == userid && u.ToStep == QuestionStepType.Processing));


            #region Filters
            if (!dto.Title.IsNullOrEmpty())
            items = items.Where(p => p.Title!.Contains(dto.Title!));
        #endregion

        int rows = 0;
        var res = await items
            .ToPaged(dto.PageNumber, dto.PageSize, out rows)
            .Select(p => new GetProcessQuestionDto
			{
                Id = p.Id,
                Title = p.Title,
                Name = p.FirstName + " " + p.LastName,
                Category = p.QuestionCategory!.Title,
                QuestionDate = p.QuestionDate!.Value.ToPersianWithoutTime(),
                Text=p.QuestionText,
                Status=p.Step!.GetDisplayName(),
                UserId=p.UserId,
                UserName=p.User!.Name,
                IsChecked=_context.PreProccesses.Any(u=>u.QuestionCycle!.QuestionId==p.Id && u.QuestionCycle.ToUserId==p.UserId && u.QuestionCycle.ToStep==QuestionStepType.Processing && u.QuestionCycle.PreProccess!=null),
                Date=_context.QuestionCycles.FirstOrDefault(u=>u.QuestionId==p.Id && u.ToStep==QuestionStepType.Processing && !p.IsRemoved)!.CreatedAt.ToPersianWithoutTime()
            })
            .ToListAsync();

        var result = new ResultOfList<GetProcessQuestionDto>
        {
            Items = res,
            Paging = new Paging(dto.PageNumber, dto.PageSize, rows)
        };

        return ResultDto<ResultOfList<GetProcessQuestionDto>>.Succeeded("ok", result);
    }
}

#region dtos
public class GetProcessQuestionRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Title { get; set; }
}
public class GetProcessQuestionDto
{
    public long? Id { get; set; }
    public string? Title { get; set; }
	public string? QuestionDate { get; set; }
	public string? Name { get; set; }
    public string? Category { get; set; }
    public bool? IsChecked { get; set; }
    public string? Text { get; set; }
    public string? Status { get; set; }
    public long? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Date { get; set; }
}
#endregion