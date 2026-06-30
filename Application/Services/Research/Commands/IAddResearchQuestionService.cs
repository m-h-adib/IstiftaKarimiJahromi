using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Application.Services.Process.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Research.Commands;

public interface IAddResearchQuestionService
{
    Task<ResultDto> ExecuteAsync(AddResearchQuestionDto dto);
}
public class AddResearchQuestionService: IAddResearchQuestionService
{
    #region members
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHostingEnvironment _environment;
    #endregion
    #region ctors
    public AddResearchQuestionService(IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor,
        IHostingEnvironment hostingEnvironment)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
        _environment = hostingEnvironment;
    }
    #endregion

    public async Task<ResultDto> ExecuteAsync(AddResearchQuestionDto dto)
    {
        long userid = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        if (!await _context.AdminSectionAccesses.AnyAsync(p => p.UserId == userid && !p.IsRemoved && p.AdminSection!.Name == "AddResearchQuestion"))
            return ResultDto.Failed("NOT ACCESS", 403);

        var cycle = await _context.QuestionCycles
            .Include(p => p.Research)
            .FirstOrDefaultAsync(p => p.QuestionId == dto.QuestionId && !p.IsRemoved && p.IsAccept == null);

        if (cycle == null)
            return ResultDto.Failed("خطا در ارسال اطلاعات");

        if (cycle.Research != null)
            return ResultDto.Failed("شما این سوال را تحقیق کرده اید و دوباره مجاز به تحقیق نمی باشید");

        string? aFile = null;
        if (dto.AnswerFile != null) {
            UploadFileToDirectory upload = new UploadFileToDirectory(_environment);
            aFile = upload.Upload(dto.AnswerFile[0], "Researches").FileNameAddress;
        }
        string? rFile = null;
        if (dto.ReasonFile != null)
        {
            UploadFileToDirectory upload = new UploadFileToDirectory(_environment);
            rFile = upload.Upload(dto.ReasonFile[0], "Researches").FileNameAddress;
        }
		string? aFile2 = null;
		if (dto.AnswerFile2 != null)
		{
			UploadFileToDirectory upload = new UploadFileToDirectory(_environment);
			aFile2 = upload.Upload(dto.AnswerFile2[0], "Researches").FileNameAddress;
		}
		string? rFile2 = null;
		if (dto.ReasonFile2 != null)
		{
			UploadFileToDirectory upload = new UploadFileToDirectory(_environment);
			rFile2 = upload.Upload(dto.ReasonFile2[0], "Researches").FileNameAddress;
		}


		await _context.Researchs.AddAsync(new Domain.Entities.Research
        {
            CreatedAt = DateTime.Now,
            AnswerText=dto.AnswerText,
            ReasonText= dto.ReasonText,
            Description = dto.Description,
            AnswerFile=aFile,
            ReasonFile=rFile,
			AnswerFile2 = aFile2,
			ReasonFile2 = rFile2,
			IsActive = true,
            IsRemoved = false,
            QuestionCycleId = cycle.Id
        });

        await _context.SaveChangesAsync();

        return ResultDto.Succeeded("تحقیق سوال انجام شد");
    }
}
#region dtos
public class AddResearchQuestionDto
{
    public long? QuestionId { get; set; }
    public string? Description { get; set; }
    public string? AnswerText { get; set; }
    public string? ReasonText { get;set; }
	public List<IFormFile>? AnswerFile { get; set; }
    public List<IFormFile>? ReasonFile { get; set; }
	public List<IFormFile>? AnswerFile2 { get; set; }
	public List<IFormFile>? ReasonFile2 { get; set; }
}
#endregion