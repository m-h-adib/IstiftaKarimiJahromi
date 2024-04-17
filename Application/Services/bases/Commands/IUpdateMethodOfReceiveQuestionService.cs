using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.bases.Commands;

public interface IUpdateMethodOfReceiveQuestionService
{
	Task<ResultDto> ExecuteAsync(UpdateMethodOfReceiveQuestionDto dto);
}


public class UpdateMethodOfReceiveQuestionService : IUpdateMethodOfReceiveQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
	#endregion
	#region ctors
	public UpdateMethodOfReceiveQuestionService(IDatabaseContext context)
	{
		_context = context;
	}
	#endregion

	public async Task<ResultDto> ExecuteAsync(UpdateMethodOfReceiveQuestionDto dto)
	{
		if (dto == null || dto.Name == null || dto.Name == "" || dto.Id == null)
			return ResultDto.Failed("لطفا شیوه دریافت سوال را وارد نمایید");

		var method = await _context.MethodOfReceiveQuestions.FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsRemoved);
		if (method == null)
			return ResultDto.Failed("شیوه دریافت سوال با این شماره یافت نشد");
		if (await _context.MethodOfReceiveQuestions.AnyAsync(p => !p.IsRemoved && p.Title == dto.Name && p.Id != dto.Id))
			return ResultDto.Failed("شیوه دریافت سوال با این نام موجود می باشد");

		method.Title = dto.Name;
		method.Order = dto.Order;
		method.UpdatedAt = DateTime.Now;

		await _context.SaveChangesAsync();

		return ResultDto.Succeeded("ok");
	}
}
#region dtos
public class UpdateMethodOfReceiveQuestionDto
{
	public long? Id { get; set; }
	public string? Name { get; set; }
	public int? Order { get; set; }
}
#endregion