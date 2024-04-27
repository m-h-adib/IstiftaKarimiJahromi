using Application.Common;
using Application.Interfaces.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Question.Quesries;

public interface IGetOneQuestionService
{
	Task<ResultDto<GetOneQuestionDto>> ExecuteAsync(long? QuestionId);
}

public class GetOneQuestionService: IGetOneQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
    #endregion
    #region ctors
    public GetOneQuestionService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

	public async Task<ResultDto<GetOneQuestionDto>> ExecuteAsync(long? QuestionId)
	{
		if (QuestionId == null)
			return ResultDto<GetOneQuestionDto>.Failed("Bad Request");

		var res = await _context.Questions
			.Where(p => !p.IsRemoved && p.Id == QuestionId)
			.Select(p => new GetOneQuestionDto
			{
				Id = p.Id,
				Address = p.Address,
				Age = p.Age,
				City = p.City,
				CountryId = p.CountryId,
				Education = (int)p.Education!,
				Email = p.Email,
				Gender = (int)p.Gender!,
				FirstName = p.FirstName,
				Job = p.Job,
				JobCategoryId = p.JobCategoryId,
				LastName = p.LastName,
				MethodOfReceiveQuestionId = p.MethodOfReceiveQuestionId,
				Mobile = p.Mobile,
				ProvinceId = p.ProvinceId,
				QuestionCategoryId = p.QuestionCategoryId,
				QuestionDate = p.QuestionDate!.Value.ToPersianWithoutTime(),
				QuestionText = p.QuestionText,
				Title = p.Title,
				Attachs = p.QuestionAttaches!=null ? p.QuestionAttaches.Select(u => new GetOneQuestionFileDto
				{
					Id = u.Id,
					Title = u.File
				}).ToList() : null
			})
			.FirstOrDefaultAsync();


		if (res == null)
			return ResultDto<GetOneQuestionDto>.Failed("سوال یافت نشد", 404);

		return ResultDto<GetOneQuestionDto>.Succeeded("ok", res);
	}
}

#region dtos
public class GetOneQuestionDto
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public string? QuestionDate { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public int? Gender { get; set; }
	public int? Age { get; set; }
	public long? CountryId { get; set; }
	public long? ProvinceId { get; set; }
	public string? City { get; set; }
	public int? Education { get; set; }
	public long? JobCategoryId { get; set; }
	public string? Job { get; set; }
	public string? Mobile { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public long? MethodOfReceiveQuestionId { get; set; }
	public long? QuestionCategoryId { get; set; }
	public string? QuestionText { get; set; }
	public List<GetOneQuestionFileDto>? Attachs { get; set; }
}
public class GetOneQuestionFileDto
{
    public long? Id { get; set; }
    public string? Title { get; set; }
}
#endregion