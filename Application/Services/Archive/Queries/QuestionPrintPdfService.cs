using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.Archive.Queries;

public interface IQuestionPrintPdfService
{
    byte[] Generate(GetQuestionPrintDto question, string format);
}

public class QuestionPrintPdfService : IQuestionPrintPdfService
{
    public byte[] Generate(GetQuestionPrintDto question, string format)
    {
        var isA4 = string.Equals(format, "A4", StringComparison.OrdinalIgnoreCase);
        var scale = isA4 ? 1f : 0.7071f;

        var pageWidth = isA4 ? PageSizes.A4.Width : PageSizes.A5.Width;
        var pageHeight = isA4 ? PageSizes.A4.Height : PageSizes.A5.Height;

        var document = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(pageWidth, pageHeight);
                page.Margin(0);
                page.PageColor(Colors.White);
                page.ContentFromRightToLeft();

                page.DefaultTextStyle(x => x
                    .FontFamily("Tahoma", "Arial")
                    .FontSize(10.5f * scale)
                    .FontColor(Colors.Black));

                page.Foreground().Column(foreground =>
                {
                    foreground.Item()
                        .Height(150 * scale)
                        .Column(top =>
                        {
                            top.Item()
                                .Height(39.5f * scale)
                                .Background("#BF935B")
                                .AlignCenter()
                                .AlignMiddle()
                                .Text("واحد پاسخ به مسائل شرعی")
                                .FontFamily("Tahoma", "Arial")
                                .Bold()
                                .FontSize(13 * scale);

                            top.Item()
                                .PaddingTop(17 * scale)
                                .PaddingLeft(75 * scale)
                                .Width(150 * scale)
                                .Column(meta =>
                                {
                                    MetaRow(meta, "شماره استفتاء", question.Id.ToString("D8"), scale);
                                    MetaRow(meta, "تاریخ وصول", question.QuestionDate, scale);
                                    MetaRow(meta, "نام و نام خانوادگی", question.Name, scale);
                                });
                        });

                    foreground.Item()
                        .AlignBottom()
                        .PaddingBottom(17 * scale)
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.DefaultTextStyle(x => x
                                .FontFamily("Tahoma", "Arial")
                                .FontSize(8.5f * scale)
                                .FontColor("#006B3C"));

                            text.Span("سامانه پیام کوتاه: ۳۰۰۰۷۴۲۸۴");
                            text.Span("  |  تلفن: ۰۲۵۳۷۷۲۴۷۶۸");
                            text.Span("  |  شبکه‌های اجتماعی: karimijahromi.ir");
                        });
                });

                page.Content()
                    .PaddingTop(154 * scale)
                    .PaddingLeft(55 * scale)
                    .PaddingRight(55 * scale)
                    .PaddingBottom(55 * scale)
                    .Column(column =>
                    {
                        column.Spacing(11 * scale);

                        if (!string.IsNullOrWhiteSpace(question.QuestionText))
                        {
                            column.Item()
                                .Text(question.QuestionText)
                                .FontFamily("Tahoma", "Arial")
                                .FontSize(11 * scale)
                                .LineHeight(1.8f)
                                .Justify();
                        }

                        foreach (var answer in question.Answers)
                        {
                            if (!string.IsNullOrWhiteSpace(answer.Answer))
                            {
                                column.Item()
                                    .PaddingTop(7 * scale)
                                    .Text(answer.Answer)
                                    .FontFamily("Tahoma", "Arial")
                                    .FontSize(11 * scale)
                                    .LineHeight(1.8f)
                                    .Justify();
                            }

                            if (!string.IsNullOrWhiteSpace(answer.Reason))
                            {
                                column.Item()
                                    .PaddingTop(4 * scale)
                                    .Text($"دلیل: {answer.Reason}")
                                    .FontFamily("Tahoma", "Arial")
                                    .FontSize(10.5f * scale)
                                    .LineHeight(1.7f)
                                    .Justify();
                            }

                            if (!string.IsNullOrWhiteSpace(answer.Description))
                            {
                                column.Item()
                                    .PaddingTop(4 * scale)
                                    .Text($"توضیحات: {answer.Description}")
                                    .FontFamily("Tahoma", "Arial")
                                    .FontSize(10.5f * scale)
                                    .LineHeight(1.7f)
                                    .Justify();
                            }
                        }
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static void MetaRow(
        ColumnDescriptor column,
        string label,
        string? value,
        float scale)
    {
        column.Item()
            .Height(22 * scale)
            .AlignRight()
            .Text(text =>
            {
                text.DefaultTextStyle(x => x
                    .FontFamily("Tahoma", "Arial")
                    .FontSize(10.5f * scale)
                    .FontColor(Colors.Black));

                text.Span($"{label}: ").Bold();
                text.Span(string.IsNullOrWhiteSpace(value) ? "-" : value);
            });
    }
}
