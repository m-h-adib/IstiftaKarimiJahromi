using Application.Common;
using Application.Interfaces.Contexts;
using Domain.Enums;
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
    #endregion
    #region ctors
    public GetArchiveQuestionService(IDatabaseContext context)
    {
        _context = context;
    }
    #endregion

    public async Task<ResultDto<ResultOfList<GetArchiveQuestionDto>>> ExecuteAsync(GetArchiveQuestionRequestDto request)
    {
        var items = _context
            .Questions
            .Where(p => !p.IsRemoved && p.Step == QuestionStepType.Archive);

        if (!string.IsNullOrEmpty(request.Title))
            items = items.Where(p => p.Title!.Contains(request.Title) || request.Title.Contains(p.Title) || request.Title.Contains(p.QuestionText!) || p.QuestionText!.Contains(request.Title));

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
            items = items.Where(p => p.QuestionCategoryId == request.QuestionCategoryId);

        if (request.JobCategoryId != request.JobCategoryId)
            items = items.Where(p => p.JobCategoryId == request.JobCategoryId);

        if (request.MethodOfReceiveQuestionId != null)
            items = items.Where(p => p.MethodOfReceiveQuestionId == request.MethodOfReceiveQuestionId);

        var res = await items
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
                QuestionDate = p.QuestionDate != null ? p.QuestionDate.Value.ToPersianWithoutTime() : null
            })
            .ToListAsync();

        var result = new ResultOfList<GetArchiveQuestionDto>
        {
            Items = res,
            Paging = new(request.PageNumber, request.PageSize, rows)
        };

        return ResultDto<ResultOfList<GetArchiveQuestionDto>>.Succeeded("ok", result);
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
}
#endregion