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

namespace Application.Services.Question.Commands;

public interface IReferenceToProcessorService
{
	Task<ResultDto> ExecuteAsync(ReferenceToProcessorDto dto);
}

public class ReferenceToProcessorService : IReferenceToProcessorService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public ReferenceToProcessorService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
    }
	#endregion

	public async Task<ResultDto> ExecuteAsync(ReferenceToProcessorDto dto)
	{
		long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

		var question = await _context.Questions
			.FirstOrDefaultAsync(p => p.Id == dto.QuestionId && !p.IsRemoved);

		if (question == null)
			return ResultDto.Failed("سوالی یافت نشد", 404);

		var cycle = await _context.QuestionCycles
			.Where(p => p.QuestionId == dto.QuestionId && p.IsAccept == null)
			.FirstOrDefaultAsync();

		if (cycle == null)
			return ResultDto.Failed("سوال در هیچ یک از مراحل نمی باشد", 400);

		if (cycle.ToStep != Domain.Enums.QuestionStepType.Add)
			return ResultDto.Failed("سوال در مرحله ثبت سوال نمی باشد");

		cycle.IsAccept = true;
		question.Step = Domain.Enums.QuestionStepType.Processing;

		await _context.QuestionCycles.AddAsync(new Domain.Entities.QuestionCycle
		{
			CreatedAt = DateTime.Now,
			FromUserId = dto.FromUser,
			ToUserId = dto.ToUser,
			UserId = userid,
			QuestionId = question.Id,
			IsRemoved = false,
			IsAccept = null,
			IsActive = true,
			FromStep = Domain.Enums.QuestionStepType.Add,
			ToStep = Domain.Enums.QuestionStepType.Processing,
			Description = dto.Description
		});

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ارجاع به پردازشگر سوال انجام شد");
	}
}

#region dtos
public class ReferenceToProcessorDto
{
    public long? QuestionId { get; set; }
    public long? FromUser { get; set; }
    public long? ToUser { get; set; }
    public string? Description { get; set; }
}
#endregion