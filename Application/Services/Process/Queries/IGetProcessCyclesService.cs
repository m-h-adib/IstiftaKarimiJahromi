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

namespace Application.Services.Process.Queries;

public interface IGetProcessCyclesService
{
	Task<ResultDto<List<GetProcessCycleDto>>> ExecuteAsync(GetProcessCycleRequestDto dto);
}

public class GetProcessCyclesService: IGetProcessCyclesService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public GetProcessCyclesService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
	}
	#endregion


	public async Task<ResultDto<List<GetProcessCycleDto>>> ExecuteAsync(GetProcessCycleRequestDto dto)
	{
		var res = await _context.QuestionCycles
			.Where(p => !p.IsRemoved)
			.Where(p => p.ToUserId == dto.UserId)
			.Where(p => p.QuestionId == dto.QuestionId)
			.Where(p => p.ToStep == Domain.Enums.QuestionStepType.Processing)
			.Select(p => new GetProcessCycleDto
			{
				Id = p.Id,
				DuplicateId = p.PreProccess != null && p.PreProccess.DuplicateId!=null ? p.PreProccess.DuplicateId : null,
				Duplicate = p.PreProccess != null && p.PreProccess.DuplicateId != null ? p.PreProccess.Duplicate!.Title : null,
				IsAccept = p.IsAccept,
				QDesctiption = p.Description,
				PDescription = p.PreProccess != null ? p.PreProccess.Description : null,
				HasProccess=p.PreProccess!=null ? true : false
			}).ToListAsync();

		return ResultDto<List<GetProcessCycleDto>>.Succeeded("ok", res);
	}
}
#region dtos
public class GetProcessCycleRequestDto
{
    public long UserId { get; set; }
    public long QuestionId { get; set; }
}
public class GetProcessCycleDto
{
    public long Id { get; set; }
    public string? QDesctiption { get; set; }
    public string? PDescription { get; set; }
    public long? DuplicateId { get; set; }
    public string? Duplicate { get; set; }
    public bool? IsAccept { get; set; }
    public bool HasProccess { get; set; }
}
#endregion