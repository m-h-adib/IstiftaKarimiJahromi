using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services.Question.Commands;

public interface IAddQuestionService
{
	Task<ResultDto> ExecuteAsync(AddQuestionDto dto);
}

public class AddQuestionService : IAddQuestionService
{
	#region members
	private readonly IDatabaseContext _context;
	private readonly IHostingEnvironment _environment;
	private readonly ITokenContents _tokenContents;
	private readonly IHttpContextAccessor _httpContextAccessor;
	#endregion
	#region ctors
	public AddQuestionService(IDatabaseContext context,
		IHostingEnvironment environment,
		ITokenContents tokenContents,
		IHttpContextAccessor httpContextAccessor)
	{
		_context = context;
		_environment = environment;
		_httpContextAccessor = httpContextAccessor;
		_tokenContents = tokenContents;
	}
	#endregion


	public async Task<ResultDto> ExecuteAsync(AddQuestionDto dto)
	{
		long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

		#region validator
		var validator = new AddQuestionValidator().Validate(dto);
		if (!validator.IsValid)
			return ResultDto.Failed(validator.ErrorMessage());
		#endregion

		List<string> files = new List<string>();

		if (dto.Attachs != null && dto.Attachs.Count > 0)
		{
			foreach (var item in dto.Attachs)
			{
				UploadFileToDirectory upload = new UploadFileToDirectory(_environment);
				files.Add(upload.Upload(item, "Files").FileNameAddress!);
			}
		}

		var res = await _context.Questions.AddAsync(new Domain.Entities.Question
		{
			Title = dto.Title,
			Address = dto.Address,
			Mobile = dto.Mobile,
			ProvinceId = dto.ProvinceId,
			Age = dto.Age,
			City = dto.City,
			CountryId = dto.CountryId,
			CreatedAt = DateTime.Now,
			Education = (EducationType)dto.Education!,
			Email = dto.Email,
			FirstName = dto.FirstName,
			Gender = (GenderType)dto.Gender!,
			IsActive = true,
			IsRemoved = false,
			Job = dto.Job,
			JobCategoryId = dto.JobCategoryId,
			LastName = dto.LastName,
			MethodOfReceiveQuestionId = dto.MethodOfReceiveQuestionId,
			QuestionCategoryId = dto.QuestionCategoryId,
			QuestionDate = dto.QuestionDate!.ToMiladi(),
			QuestionCode = new Guid(),
			QuestionText = dto.QuestionText,
			UserId = userid,
			Step = QuestionStepType.Add,
			QuestionAttaches = files.Select(p => new QuestionAttach
			{
				CreatedAt = DateTime.Now,
				File = p,
				IsActive = true,
				IsRemoved = false
			}).ToList(),
			QuestionCycles = new List<QuestionCycle>() { 
				new QuestionCycle
				{
					FromStep = QuestionStepType.None,
					ToStep = QuestionStepType.Add,
					FromUserId = null,
					ToUserId = userid,
					UserId = userid,
					CreatedAt = DateTime.Now,
					IsAccept = null,
					IsRemoved = false,
					IsActive = true,
					Description="ثبت اولیه سوال"
				}
			}
		});


		await _context.SaveChangesAsync();

		
		return ResultDto.Succeeded("ok");
	}
}

#region dtos
public class AddQuestionDto
{
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
	public List<IFormFile>? Attachs { get; set; }
}
#endregion

#region validations
public class AddQuestionValidator : AbstractValidator<AddQuestionDto>
{
	public AddQuestionValidator()
	{
		RuleFor(p => p.Title).NotNull().WithMessage("عنوان سوال الزامیست");
		//RuleFor(p => p.QuestionDate).NotNull().WithMessage("تاریخ دریافت سوال الزامیست")
		//	.Must(p => p.Split("/").Length == 3).WithMessage("تاریخ را به درستی وارد نمایید"); ;
		//RuleFor(p => p.FirstName).NotNull().WithMessage("نام الزامیست");
		//RuleFor(p => p.LastName).NotNull().WithMessage("نام خانوادگی الزامیست");
		//RuleFor(p => p.Gender).NotNull().WithMessage("جنسیت الزامیست");
		//RuleFor(p => p.Age).NotNull().WithMessage("سن الزامیست");
		//RuleFor(p => p.CountryId).NotNull().WithMessage("کشور الزامیست");
		//RuleFor(p => p.ProvinceId).NotNull().WithMessage("استان الزامیست");
		//RuleFor(p => p.City).NotNull().WithMessage("شهر الزامیست");
		//RuleFor(p => p.Education).NotNull().WithMessage("تحصیلات الزامیست");
		//RuleFor(p => p.JobCategoryId).NotNull().WithMessage("دسته شغلی الزامیست");
		//RuleFor(p => p.Job).NotNull().WithMessage("شغل الزامیست");
		//RuleFor(p => p.Mobile).NotNull().WithMessage("شماره همراه الزامیست")
		//	.Matches(new Regex(@"09(1[0-9]|3[1-9])?[0-9]{3}?[0-9]{4}")).WithMessage("شماره همراه را بدرستی و به صورت عدد لاتین 11 رقمی وارد نمایید")
		//	.MinimumLength(11).WithMessage("شماره تماس باید 11 رقم باشد")
		//	.MaximumLength(11).WithMessage("شماره تماس باید 11 رقم باشد");
		//RuleFor(p => p.MethodOfReceiveQuestionId).NotNull().WithMessage("شیوه دریافت سوال الزامیست");
		RuleFor(p => p.QuestionCategoryId).NotNull().WithMessage("دسته سوال الزامیست");
		RuleFor(p => p.QuestionText).NotNull().WithMessage("متن سوال الزامیست");
	}
}
#endregion