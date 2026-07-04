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

namespace Application.Services.Process.Commands;

public interface IAddProcessQuestionService
{
	Task<ResultDto> ExecuteAsync(AddProcessQuestionDto dto);
}

public class AddProcessQuestionService: IAddProcessQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public AddProcessQuestionService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(AddProcessQuestionDto dto)
	{
		long userid=_tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);


		if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "AddProcessQuestion"))
			return ResultDto.Failed("NOT ACCESS", 403);

		var cycle = await _context.QuestionCycles
			.Include(p=>p.PreProccess)
			.Include(p=>p.Question)
			.FirstOrDefaultAsync(p => p.QuestionId == dto.QuestionId && !p.IsRemoved && p.IsAccept==null);



		if (cycle == null)
			return ResultDto.Failed("خطا در ارسال اطلاعات");

		if (cycle.PreProccess != null)
			return ResultDto.Failed("شما این سوال را پردازش کرده اید و دوباره مجاز به پردازش نمی باشید");

		await _context.PreProccesses.AddAsync(new Domain.Entities.PreProccess
		{
			CreatedAt = DateTime.Now,
			DuplicateId = dto.DuplicateId,
			Description = dto.Description,
			IsActive = true,
			IsRemoved = false,
			QuestionCycleId = cycle.Id
		});

		cycle.Question.DuplicateId=dto.DuplicateId;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("پردازش سوال انجام شد");
	}
}

#region dtos
public class AddProcessQuestionDto
{
    public long? QuestionId { get; set; }
    public string? Description { get; set; }
    public long? DuplicateId { get; set; }
}
#endregion