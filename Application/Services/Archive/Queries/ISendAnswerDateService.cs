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

namespace Application.Services.Archive.Queries;

public interface ISendAnswerDateService
{
	Task<ResultDto> ExecuteAsync(SendAnswerDateDto dto);
}
public class SendAnswerDateService: ISendAnswerDateService
{
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _contextAccessor;
	public SendAnswerDateService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_contextAccessor = httpContextAccessor;
	}

	public async Task<ResultDto> ExecuteAsync(SendAnswerDateDto dto)
	{
		long userid = _tokenContents.GetUserId(_contextAccessor.HttpContext!.User);

		if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "SetAnswerDate"))
			return ResultDto<ResultOfList<GetArchiveQuestionDto>>.Failed("NOT ACCESS", 403);


		var q=await _context.Questions
			.Where(p=>p.Step==Domain.Enums.QuestionStepType.Archive && p.Id==dto.QuestionId && !p.IsRemoved)
			.FirstOrDefaultAsync();

		if (q == null)
			return ResultDto.Failed("سوال در وضعیتی نیست که بتوان تاریخ ارسال برای آن ثبت کرد");

		q.SendAnswerDate = dto.AnswerDate.ToMiladi();

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
public class SendAnswerDateDto
{
	public string? AnswerDate { get; set; }
	public long? QuestionId { get; set; }
}