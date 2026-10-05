using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.Archive.Queries;

public interface IQuestionPrintPdfService
{
    byte[] Generate(GetQuestionPrintDto question, string format, string? stampSvg);
}

public class QuestionPrintPdfService : IQuestionPrintPdfService
{
    public byte[] Generate(GetQuestionPrintDto question, string format, string? stampSvg)
    {
        var isA4 = string.Equals(format, "A4", StringComparison.OrdinalIgnoreCase);
        var pageSize = isA4 ? PageSizes.A4 : PageSizes.A5;
        var margin = isA4 ? 40 : 34;

        var document = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(pageSize.Portrait());
                page.Margin(margin);
                page.PageColor(Colors.White);
                page.ContentFromRightToLeft();
                page.DefaultTextStyle(x => x
                    .FontFamily("Tahoma", "Arial")
                    .FontSize(9.5f)
                    .FontColor(Colors.Black));

                page.Content().Column(column =>
                {
                    column.Spacing(12);

                    column.Item()
                        .BorderBottom(1)
                        .PaddingBottom(8)
                        .Column(header =>
                        {
                            header.Item()
                                .Text("استفتاء")
                                .FontSize(17)
                                .Bold()
                                .AlignCenter();

                            header.Item()
                                .PaddingTop(2)
                                .Text($"کد سوال: {question.Id:D8}")
                                .FontSize(9)
                                .AlignCenter();
                        });

                    column.Item().Column(meta =>
                    {
                        MetaRow(meta, "عنوان", question.Title);
                        MetaRow(meta, "تاریخ ثبت", question.QuestionDate);
                        MetaRow(meta, "نام", question.Name);
                        MetaRow(meta, "شماره همراه", question.Mobile);
                        MetaRow(meta, "دسته سوال", question.QuestionCategory);
                        MetaRow(meta, "تاریخ پاسخ", question.AnswerDate);

                        if (!string.IsNullOrWhiteSpace(question.Address))
                            MetaRow(meta, "آدرس", question.Address);
                    });

                    column.Item()
                        .Column(section =>
                        {
                            SectionTitle(section, "متن استفتاء");
                            section.Item()
                                .Text(question.QuestionText ?? "-")
                                .Justify();
                        });

                    if (question.Answers.Count > 0)
                    {
                        column.Item().Column(section =>
                        {
                            SectionTitle(section, "پاسخ");

                            foreach (var answer in question.Answers)
                            {
                                section.Item()
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Medium)
                                    .CornerRadius(3)
                                    .Padding(8)
                                    .Column(answerColumn =>
                                    {
                                        var metaText = string.Join(
                                            " | ",
                                            new[]
                                            {
                                                string.IsNullOrWhiteSpace(answer.Language) ? null : $"زبان: {answer.Language}",
                                                string.IsNullOrWhiteSpace(answer.EditorUser) ? null : $"تنظیم‌کننده: {answer.EditorUser}",
                                                string.IsNullOrWhiteSpace(answer.Date) ? null : answer.Date
                                            }.Where(x => x != null));

                                        if (!string.IsNullOrWhiteSpace(metaText))
                                        {
                                            answerColumn.Item()
                                                .Text(metaText)
                                                .FontSize(7.5f)
                                                .FontColor(Colors.Grey.Darken2);
                                        }

                                        answerColumn.Item()
                                            .PaddingTop(4)
                                            .Text(answer.Answer ?? "-")
                                            .Justify();

                                        if (!string.IsNullOrWhiteSpace(answer.Reason))
                                        {
                                            answerColumn.Item()
                                                .PaddingTop(5)
                                                .Text(text =>
                                                {
                                                    text.Span("دلیل: ").Bold();
                                                    text.Span(answer.Reason);
                                                });
                                               // .Justify();
                                        }

                                        if (!string.IsNullOrWhiteSpace(answer.Description))
                                        {
                                            answerColumn.Item()
                                                .PaddingTop(5)
                                                .Text(text =>
                                                {
                                                    text.Span("توضیحات: ").Bold();
                                                    text.Span(answer.Description);
                                                });
                                                //.Justify();
                                        }
                                    });
                            }
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(stampSvg))
                    {
                        column.Item()
                            .PaddingTop(10)
                            .AlignLeft()
                            .Width(90)
                            .Svg(stampSvg)
                            .FitWidth();
                    }
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void MetaRow(ColumnDescriptor column, string label, string? value)
    {
        column.Item()
            .PaddingBottom(3)
            .Text(text =>
            {
                text.Span($"{label}: ").Bold();
                text.Span(string.IsNullOrWhiteSpace(value) ? "-" : value);
            });
    }

    private static void SectionTitle(ColumnDescriptor column, string title)
    {
        column.Item()
            .BorderBottom(0.7f)
            .PaddingBottom(3)
            .Text(title)
            .Bold()
            .FontSize(10.5f);
    }
}
