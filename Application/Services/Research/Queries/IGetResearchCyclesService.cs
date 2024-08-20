using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Queries;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Research.Queries;

public interface IGetResearchCyclesService
{
    Task<ResultDto<List<GetResearchCycleDto>>> ExecuteAsync(GetResearchCycleRequestDto dto);
}

public class GetResearchCyclesService: IGetResearchCyclesService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    #endregion
    #region ctors
    public GetResearchCyclesService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }
    #endregion


    public async Task<ResultDto<List<GetResearchCycleDto>>> ExecuteAsync(GetResearchCycleRequestDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);


        var res = await _context.QuestionCycles
            .Where(p => !p.IsRemoved)
            .Where(p => p.ToUserId == userid)
            .Where(p => p.QuestionId == dto.QuestionId)
            .Where(p => p.ToStep == Domain.Enums.QuestionStepType.Research)
            .Select(p => new GetResearchCycleDto
            {
                Id = p.Id,
                ResearchId=p.Research!=null ? p.Research.Id : null,
                DuplicateId = p.PreProccess != null && p.PreProccess.DuplicateId != null ? p.PreProccess.DuplicateId : null,
                Duplicate = p.PreProccess != null && p.PreProccess.DuplicateId != null ? p.PreProccess.Duplicate!.Title : null,
                IsAccept = p.IsAccept,
                QDesctiption = p.Description,
                PDescription = p.Research != null ? p.Research.Description : null,
                HasProccess = p.Research != null ? true : false
            }).ToListAsync();

        return ResultDto<List<GetResearchCycleDto>>.Succeeded("ok", res);
    }
}
#region dtos
public class GetResearchCycleRequestDto
{
    public long QuestionId { get; set; }
}
public class GetResearchCycleDto
{
    public long Id { get; set; }
    public long? ResearchId { get; set; }
    public string? QDesctiption { get; set; }
    public string? PDescription { get; set; }
    public long? DuplicateId { get; set; }
    public string? Duplicate { get; set; }
    public bool? IsAccept { get; set; }
    public bool HasProccess { get; set; }
}
#endregion