using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Archive.Queries;

public interface IGetArchiveQuestionService
{
    Task<ResultDto<ResultOfList<GetArchiveQuestionDto>>> ExecuteAsync(GetArchiveQuestionRequestDto request);
}
public class GetArchiveQuestionService: IGetArchiveQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public GetArchiveQuestionService(IDatabaseContext context,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_tokenContents = tokenContents;
		_httpContextAccessor = httpContextAccessor;
	}
	#endregion
	public async Task<ResultDto<ResultOfList<GetArchiveQuestionDto>>> ExecuteAsync(GetArchiveQuestionRequestDto request)
    {
		long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

		if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "Archives"))
			return ResultDto<ResultOfList<GetArchiveQuestionDto>>.Failed("NOT ACCESS", 403);

        var show_all_access = await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "ShowAllArchive");

		var items = _context
            .Questions
            .Where(p => !p.IsRemoved && (show_all_access==false ? p.Step == QuestionStepType.Archive : true));

      
        if (!string.IsNullOrEmpty(request.Title))
        {
            List<string> keys = request.Title.Split(" ").ToList();


            switch (keys.Count)
            {
                case 1:
                    items = items.Where(p => p.Title.Contains(request.Title) || p.QuestionText.Contains(request.Title));
                    break;
                case 2:
                    items = items.Where(p => p.Title.Contains(request.Title) || p.QuestionText.Contains(request.Title) ||
                    ((p.Title.Contains(keys[0]) || p.QuestionText.Contains(keys[0])) &&
                    (p.Title.Contains(keys[1]) || p.QuestionText.Contains(keys[1]))));
                    break;
                case 3:
                    items = items.Where(p => p.Title.Contains(request.Title) || p.QuestionText.Contains(request.Title) ||
                    ((p.Title.Contains(keys[0]) || p.QuestionText.Contains(keys[0])) &&
                    (p.Title.Contains(keys[1]) || p.QuestionText.Contains(keys[1])) &&
                    (p.Title.Contains(keys[2]) || p.QuestionText.Contains(keys[2]))));
                    break;
            }
        }

        if (!string.IsNullOrEmpty(request.FirstName))
            items = items.Where(p => p.FirstName!.Contains(request.FirstName) || request.FirstName.Contains(p.FirstName));

        if (!string.IsNullOrEmpty(request.LastName))
            items = items.Where(p => p.LastName!.Contains(request.LastName) || request.LastName.Contains(p.LastName));

        if (!string.IsNullOrEmpty(request.City))
            items = items.Where(p => p.City!.Contains(request.City) || request.City.Contains(p.City));

        if (request.Education != null)
            items = items.Where(p => p.Education == (EducationType)request.Education);

        if (request.ProvinceId != null)
            items = items.Where(p => p.ProvinceId == request.ProvinceId);

        if (request.CountryId != null)
            items = items.Where(p => p.CountryId == request.CountryId);

        if (!string.IsNullOrEmpty(request.Mobile))
            items = items.Where(p => p.Mobile!.Contains(request.Mobile) || request.Mobile.Contains(p.Mobile));

        if (request.FromAge != null)
            items = items.Where(p => p.Age >= request.FromAge);

        if (request.ToAge != null)
            items = items.Where(p => p.Age <= request.ToAge);

        if (request.Gender != null)
            items = items.Where(p => p.Gender == (GenderType)request.Gender);

        if (!string.IsNullOrEmpty(request.FromDate))
            items = items.Where(p => p.QuestionDate >= request.FromDate.ToMiladi());

		if (!string.IsNullOrEmpty(request.ToDate))
			items = items.Where(p => p.QuestionDate <= request.ToDate.ToMiladi());

        if (request.QuestionCategoryId != null)
        {
			var categoryIds = await GetDescendantIds(request.QuestionCategoryId.Value);
			items = items.Where(p => categoryIds.Contains(p.QuestionCategoryId.Value));
        }

        if (request.JobCategoryId != request.JobCategoryId)
            items = items.Where(p => p.JobCategoryId == request.JobCategoryId);

        if (request.MethodOfReceiveQuestionId != null)
            items = items.Where(p => p.MethodOfReceiveQuestionId == request.MethodOfReceiveQuestionId);

        if(request.Id != null)
            items=items.Where(p=>p.Id == request.Id);

      
        var res = await items
            .OrderByDescending(p=>p.CreatedAt)
            .ToPaged(request.PageNumber, request.PageSize, out int rows)
            .Select(p => new GetArchiveQuestionDto
            {
                Address = p.Address,
                Age = p.Age,
                City = p.City,
                Country = p.CountryId != null ? p.Country!.Title : null,
                Title = p.Title,
                DuplicateId = p.DuplicateId,
                Education = p.Education != null ? p.Education.GetDisplayName() : null,
                Email = p.Email,
                Gender = p.Gender != null ? p.Gender.GetDisplayName() : null,
                Id = p.Id,
                Job = p.Job,
                JobCategory = p.JobCategoryId != null ? p.JobCategory!.Title : null,
                MethodOfReceiveQuestion = p.MethodOfReceiveQuestionId != null ? p.MethodOfReceiveQuestion!.Title : null,
                Mobile = p.Mobile,
                Name = p.FirstName + " " + p.LastName,
                Province = p.ProvinceId != null ? p.Province!.Title : null,
                QuestionCategory = p.QuestionCategoryId != null ? p.QuestionCategory!.Title : null,
                QuestionDate = p.QuestionDate != null ? p.QuestionDate.Value.ToPersianWithoutTime() : null,
                State=p.Step!=null ? p.Step.GetDisplayName() : null,
                AnswerDate=p.SendAnswerDate!=null ? p.SendAnswerDate.Value.ToPersianWithoutTime(): null,
            })
            .ToListAsync();

        var result = new ResultOfList<GetArchiveQuestionDto>
        {
            Items = res,
            Paging = new(request.PageNumber, request.PageSize, rows)
        };

        return ResultDto<ResultOfList<GetArchiveQuestionDto>>.Succeeded("ok", result);
	}

	private async Task<List<long>> GetDescendantIds(long categoryId)
	{
		var categories = await _context.QuestionCategories.ToListAsync();

		var result = new List<long> { categoryId };

		void AddChildren(long parentId)
		{
			var children = categories
				.Where(x => x.ParentId == parentId)
				.Select(x => x.Id);

			foreach (var child in children)
			{
				result.Add(child);
				AddChildren(child);
			}
		}

		AddChildren(categoryId);

		return result;
	}
}

#region dtos
public class GetArchiveQuestionRequestDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Title { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Gender { get; set; }
    public int? FromAge { get; set; }
    public int? ToAge { get; set; }
    public long? CountryId { get; set; }
    public long? ProvinceId { get; set; }
    public string? City { get; set; }
    public int? Education { get; set; }
    public long? JobCategoryId { get; set; }
    public string? Mobile { get; set; }
	public long? MethodOfReceiveQuestionId { get; set; }
	public long? QuestionCategoryId { get; set; }
	public long? Id { get; set; }
}

public class GetArchiveQuestionDto
{
    public long? Id { get; set; }
    public string? Title { get; set; }
    public string? QuestionDate { get; set; }
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public int? Age { get; set; }
	public string? City { get; set; }
	public string? Education { get; set; }
	public string? Job { get; set; }
	public string? Mobile { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public long? DuplicateId { get; set; }
	public string? Country { get; set; }
    public string? Province { get; set; }
    public string? JobCategory { get; set; }
	public string? MethodOfReceiveQuestion { get; set; }
	public string? QuestionCategory { get;set; }
	public string? State { get; set; }
	public string? AnswerDate { get; set; }
}
#endregion