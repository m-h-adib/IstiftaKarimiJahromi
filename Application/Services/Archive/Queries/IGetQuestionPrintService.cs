using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Archive.Queries;

public interface IGetQuestionPrintService
{
    Task<ResultDto<GetQuestionPrintDto>> ExecuteAsync(long? questionId);
}

public class GetQuestionPrintService : IGetQuestionPrintService
{
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetQuestionPrintService(
        IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResultDto<GetQuestionPrintDto>> ExecuteAsync(long? questionId)
    {
        if (questionId == null)
            return ResultDto<GetQuestionPrintDto>.Failed("Bad Request");

        var userId = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);

        var hasArchiveAccess = await _context.AdminSectionAccesses
            .AnyAsync(p => p.UserId == userId &&
                           !p.IsRemoved &&
                           p.AdminSection!.Name == "Archives");

        if (!hasArchiveAccess)
            return ResultDto<GetQuestionPrintDto>.Failed("NOT ACCESS", 403);

        var question = await _context.Questions
            .Where(p => !p.IsRemoved && p.Id == questionId)
            .Select(p => new GetQuestionPrintDto
            {
                Id = p.Id,
                Code = p.QuestionCode,
                Title = p.Title,
                QuestionDate = p.QuestionDate != null
                    ? p.QuestionDate.Value.ToPersianWithoutTime()
                    : null,
                Name = p.FirstName + " " + p.LastName,
                Mobile = p.Mobile,
                Address = p.Address,
                QuestionText = p.QuestionText,
                QuestionCategory = p.QuestionCategoryId != null
                    ? p.QuestionCategory!.Title
                    : null,
                AnswerDate = p.SendAnswerDate != null
                    ? p.SendAnswerDate.Value.ToPersianWithoutTime()
                    : null
            })
            .FirstOrDefaultAsync();

        if (question == null)
            return ResultDto<GetQuestionPrintDto>.Failed("استفتاء یافت نشد", 404);

        question.Answers = await _context.Editors
            .Where(p => p.QuestionCycle!.QuestionId == questionId &&
                        p.IsActive &&
                        !p.IsRemoved &&
                        p.IsAccept == true &&
                        !p.QuestionCycle!.IsRemoved &&
                        !p.QuestionCycle!.Question!.IsRemoved &&
                        p.Language != null &&
                        p.Language.Title == "فارسی")
            .Select(p => new GetQuestionPrintAnswerDto
            {
                AnswerText = p.ReasonText
            })
            .ToListAsync();

        return ResultDto<GetQuestionPrintDto>.Succeeded("ok", question);
    }
}

public class GetQuestionPrintDto
{
    public long Id { get; set; }
    public Guid? Code { get; set; }
    public string? Title { get; set; }
    public string? QuestionDate { get; set; }
    public string? Name { get; set; }
    public string? Mobile { get; set; }
    public string? Address { get; set; }
    public string? QuestionText { get; set; }
    public string? QuestionCategory { get; set; }
    public string? AnswerDate { get; set; }
    public List<GetQuestionPrintAnswerDto> Answers { get; set; } = new();
}

public class GetQuestionPrintAnswerDto
{
    public string? AnswerText { get; set; }
}
