using Application.Common;
using Application.Interfaces.Contexts;
using Application.Services.Authentication.Methods;
using ClosedXML.Excel;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Archive.Queries;

public interface IExportArchiveQuestionsExcelService
{
    Task<ResultDto<byte[]>> ExecuteAsync(GetArchiveQuestionRequestDto request);
}

public class ExportArchiveQuestionsExcelService : IExportArchiveQuestionsExcelService
{
    private readonly IDatabaseContext _context;
    private readonly ITokenContents _tokenContents;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExportArchiveQuestionsExcelService(
        IDatabaseContext context,
        ITokenContents tokenContents,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenContents = tokenContents;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResultDto<byte[]>> ExecuteAsync(GetArchiveQuestionRequestDto request)
    {
        var userId = _tokenContents.GetUserId(_httpContextAccessor.HttpContext!.User);
        var hasArchiveAccess = await _context.AdminSectionAccesses.AnyAsync(p =>
            p.UserId == userId && !p.IsRemoved && p.AdminSection!.Name == "Archives");

        if (!hasArchiveAccess)
            return ResultDto<byte[]>.Failed("NOT ACCESS", 403);

        var showAllAccess = await _context.AdminSectionAccesses.AnyAsync(p =>
            p.UserId == userId && !p.IsRemoved && p.AdminSection!.Name == "ShowAllArchive");

        var items = _context.Questions
            .AsNoTracking()
            .Where(p => !p.IsRemoved &&
                (showAllAccess || p.Step == QuestionStepType.Archive));

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            var keys = request.Title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (keys.Length == 1)
            {
                items = items.Where(p => p.Title!.Contains(request.Title) ||
                                         p.QuestionText!.Contains(request.Title));
            }
            else if (keys.Length is 2 or 3)
            {
                var title = request.Title;
                var key0 = keys[0];
                var key1 = keys[1];
                if (keys.Length == 2)
                    items = items.Where(p => p.Title!.Contains(title) ||
                        p.QuestionText!.Contains(title) ||
                        ((p.Title!.Contains(key0) || p.QuestionText!.Contains(key0)) &&
                         (p.Title!.Contains(key1) || p.QuestionText!.Contains(key1))));
                else
                {
                    var key2 = keys[2];
                    items = items.Where(p => p.Title!.Contains(title) ||
                        p.QuestionText!.Contains(title) ||
                        ((p.Title!.Contains(key0) || p.QuestionText!.Contains(key0)) &&
                         (p.Title!.Contains(key1) || p.QuestionText!.Contains(key1)) &&
                         (p.Title!.Contains(key2) || p.QuestionText!.Contains(key2))));
                }
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
            items = items.Where(p => p.QuestionCategoryId.HasValue &&
                                     categoryIds.Contains(p.QuestionCategoryId.Value));
        }

        if (request.JobCategoryId != null)
            items = items.Where(p => p.JobCategoryId == request.JobCategoryId);
        if (request.MethodOfReceiveQuestionId != null)
            items = items.Where(p => p.MethodOfReceiveQuestionId == request.MethodOfReceiveQuestionId);
        if (request.Id != null)
            items = items.Where(p => p.Id == request.Id);

        var questions = await items
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new
            {
                p.Id,
                p.QuestionCode,
                p.Title,
                p.QuestionText,
                p.QuestionDate,
                p.SendAnswerDate,
                p.FirstName,
                p.LastName,
                p.Gender,
                p.Age,
                p.City,
                p.Education,
                p.Job,
                p.Mobile,
                p.Email,
                p.Address,
                p.DuplicateId,
                p.Step,
                p.CreatedAt,
                Country = p.Country == null ? null : p.Country.Title,
                Province = p.Province == null ? null : p.Province.Title,
                JobCategory = p.JobCategory == null ? null : p.JobCategory.Title,
                ReceiveMethod = p.MethodOfReceiveQuestion == null ? null : p.MethodOfReceiveQuestion.Title,
                Category = p.QuestionCategory == null ? null : p.QuestionCategory.Title
            })
            .ToListAsync();

        var questionIds = questions.Select(p => p.Id).ToList();

        var answers = await _context.Editors
            .AsNoTracking()
            .Where(p => p.IsActive && !p.IsRemoved && p.IsAccept == true &&
                        p.QuestionCycle != null && !p.QuestionCycle.IsRemoved &&
                        p.QuestionCycle.QuestionId != 0 &&
                        questionIds.Contains(p.QuestionCycle.QuestionId))
            .Select(p => new
            {
                QuestionId = p.QuestionCycle!.QuestionId,
                Language = p.Language == null ? null : p.Language.Title,
                p.AnswerText,
                p.ReasonText,
                p.Description,
                Editor = p.QuestionCycle!.ToUser == null ? null : p.QuestionCycle.ToUser.Name,
                p.CreatedAt
            })
            .OrderBy(p => p.QuestionId)
            .ThenBy(p => p.CreatedAt)
            .ToListAsync();

        var attachments = await _context.QuestionAttaches
            .AsNoTracking()
            .Where(p => !p.IsRemoved && p.QuestionId.HasValue && questionIds.Contains(p.QuestionId.Value))
            .Select(p => new { QuestionId = p.QuestionId!.Value, p.File })
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var questionSheet = workbook.Worksheets.Add("سوالات");
        var questionHeaders = new[]
        {
            "شناسه", "کد رهگیری", "عنوان سوال", "متن کامل سوال", "تاریخ ثبت سوال",
            "تاریخ ارسال پاسخ", "نام", "نام خانوادگی", "جنسیت", "سن", "شهر",
            "تحصیلات", "شغل", "کشور", "استان", "دسته شغلی", "روش دریافت سوال",
            "دسته سوال", "شماره همراه", "ایمیل", "نشانی", "مرحله", "شناسه سوال اصلی",
            "تاریخ ایجاد رکورد"
        };
        WriteHeader(questionSheet, questionHeaders);

        for (var i = 0; i < questions.Count; i++)
        {
            var q = questions[i];
            var row = i + 2;
            var values = new object?[]
            {
                q.Id, q.QuestionCode?.ToString(), q.Title, q.QuestionText,
                q.QuestionDate?.ToPersianWithoutTime(), q.SendAnswerDate?.ToPersianWithoutTime(),
                q.FirstName, q.LastName, q.Gender?.GetDisplayName(), q.Age, q.City,
                q.Education?.GetDisplayName(), q.Job, q.Country, q.Province, q.JobCategory,
                q.ReceiveMethod, q.Category, q.Mobile, q.Email, q.Address,
                q.Step?.GetDisplayName(), q.DuplicateId, q.CreatedAt.ToString("yyyy/MM/dd HH:mm")
            };
            for (var col = 0; col < values.Length; col++)
                questionSheet.Cell(row, col + 1).Value = values[col]?.ToString() ?? string.Empty;
        }
        FormatSheet(questionSheet, questions.Count + 1, questionHeaders.Length);
        questionSheet.SheetView.FreezeRows(1);

        var answerSheet = workbook.Worksheets.Add("پاسخ‌ها و دلایل");
        var answerHeaders = new[]
        {
            "شناسه سوال", "عنوان سوال", "زبان", "متن کامل پاسخ", "دلیل پاسخ",
            "توضیحات", "پاسخ‌دهنده", "تاریخ ثبت پاسخ"
        };
        WriteHeader(answerSheet, answerHeaders);
        var questionTitles = questions.ToDictionary(p => p.Id, p => p.Title ?? string.Empty);
        for (var i = 0; i < answers.Count; i++)
        {
            var a = answers[i];
            var row = i + 2;
            var values = new object?[]
            {
                a.QuestionId, questionTitles.GetValueOrDefault(a.QuestionId), a.Language,
                a.AnswerText, a.ReasonText, a.Description, a.Editor,
                a.CreatedAt.ToPersianWithoutTime()
            };
            for (var col = 0; col < values.Length; col++)
                answerSheet.Cell(row, col + 1).Value = values[col]?.ToString() ?? string.Empty;
        }
        FormatSheet(answerSheet, answers.Count + 1, answerHeaders.Length);
        answerSheet.SheetView.FreezeRows(1);

        var attachmentSheet = workbook.Worksheets.Add("پیوست‌ها");
        var attachmentHeaders = new[] { "شناسه سوال", "عنوان سوال", "نام یا مسیر فایل" };
        WriteHeader(attachmentSheet, attachmentHeaders);
        for (var i = 0; i < attachments.Count; i++)
        {
            var a = attachments[i];
            var row = i + 2;
            attachmentSheet.Cell(row, 1).Value = a.QuestionId;
            attachmentSheet.Cell(row, 2).Value = questionTitles.GetValueOrDefault(a.QuestionId) ?? string.Empty;
            attachmentSheet.Cell(row, 3).Value = a.File ?? string.Empty;
        }
        FormatSheet(attachmentSheet, attachments.Count + 1, attachmentHeaders.Length);
        attachmentSheet.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return ResultDto<byte[]>.Succeeded("ok", stream.ToArray());
    }

    private async Task<List<long>> GetDescendantIds(long categoryId)
    {
        var categories = await _context.QuestionCategories
            .AsNoTracking()
            .Select(x => new { x.Id, x.ParentId })
            .ToListAsync();

        var result = new List<long> { categoryId };
        void AddChildren(long parentId)
        {
            foreach (var child in categories.Where(x => x.ParentId == parentId).Select(x => x.Id))
            {
                if (result.Contains(child))
                    continue;
                result.Add(child);
                AddChildren(child);
            }
        }

        AddChildren(categoryId);
        return result;
    }

    private static void WriteHeader(IXLWorksheet sheet, string[] headers)
    {
        for (var i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).Value = headers[i];

        var header = sheet.Range(1, 1, 1, headers.Length);
        header.Style.Font.Bold = true;
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#365F91");
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        sheet.RightToLeft = true;
        sheet.Row(1).Height = 28;
    }

    private static void FormatSheet(IXLWorksheet sheet, int lastRow, int lastColumn)
    {
        if (lastRow > 1)
        {
            var body = sheet.Range(2, 1, lastRow, lastColumn);
            body.Style.Alignment.WrapText = true;
            body.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
            body.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            body.Style.Border.BottomBorder = XLBorderStyleValues.Hair;
        }

        //sheet.SheetView.ShowGridLines = false;
        sheet.Columns().AdjustToContents(1, 60);
        sheet.Column(1).Width = 14;
        if (lastColumn >= 4)
            sheet.Column(4).Width = 65;
        if (sheet.Name == "پاسخ‌ها و دلایل")
        {
            sheet.Column(4).Width = 65;
            sheet.Column(5).Width = 55;
            sheet.Column(6).Width = 45;
        }
        sheet.Rows(2, Math.Max(2, lastRow)).Height = 48;
        sheet.Range(1, 1, Math.Max(1, lastRow), lastColumn).SetAutoFilter();
    }
}
