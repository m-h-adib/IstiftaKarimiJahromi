using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Application.Services.Archive.Queries;

public interface IQuestionPrintPdfService
{
    byte[] Generate(GetQuestionPrintDto question, string format, string templateSvg);
}

public class QuestionPrintPdfService : IQuestionPrintPdfService
{
    public byte[] Generate(
        GetQuestionPrintDto question,
        string format,
        string templateSvg)
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

                page.Background()
                    .Svg(templateSvg);

                page.DefaultTextStyle(x => x
                    .FontFamily("Tahoma", "Arial")
                    .FontSize(10.5f * scale)
                    .FontColor(Colors.Black));

                // Only dynamic values are drawn over the reference template.
                // The template itself contains the title, ornaments, fixed labels and footer.
                page.Foreground()
                    .PaddingTop(51 * scale)
                    .PaddingLeft(90 * scale)
                    .PaddingRight(375 * scale)
                    .Column(meta =>
                    {
                        MetaValue(meta, question.Id.ToString("D8"), scale);
                        MetaValue(meta, question.QuestionDate, scale);
                        MetaValue(meta, question.Name, scale);
                    });

                page.Content()
                    .PaddingTop(151 * scale)
                    .PaddingLeft(65 * scale)
                    .PaddingRight(65 * scale)
                    .PaddingBottom(70 * scale)
                    .Column(column =>
                    {
                        column.Spacing(10 * scale);

                        if (!string.IsNullOrWhiteSpace(question.QuestionText))
                        {
                            column.Item()
                                .Text(text =>
                                {
                                    text.DefaultTextStyle(x => x
                                        .FontFamily("Tahoma", "Arial")
                                        .FontSize(11 * scale)
                                        .LineHeight(1.8f));

                                    text.Justify();
                                    text.Span(question.QuestionText);
                                });
                        }

                        foreach (var answer in question.Answers)
                        {
                            if (!string.IsNullOrWhiteSpace(answer.Answer))
                            {
                                column.Item()
                                    .PaddingTop(6 * scale)
                                    .Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x
                                            .FontFamily("Tahoma", "Arial")
                                            .FontSize(11 * scale)
                                            .LineHeight(1.8f));

                                        text.Justify();
                                        text.Span(answer.Answer);
                                    });
                            }

                            if (!string.IsNullOrWhiteSpace(answer.Reason))
                            {
                                column.Item()
                                    .PaddingTop(4 * scale)
                                    .Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x
                                            .FontFamily("Tahoma", "Arial")
                                            .FontSize(10.5f * scale)
                                            .LineHeight(1.7f));

                                        text.Justify();
                                        text.Span("دلیل: ").Bold();
                                        text.Span(answer.Reason);
                                    });
                            }

                            if (!string.IsNullOrWhiteSpace(answer.Description))
                            {
                                column.Item()
                                    .PaddingTop(4 * scale)
                                    .Text(text =>
                                    {
                                        text.DefaultTextStyle(x => x
                                            .FontFamily("Tahoma", "Arial")
                                            .FontSize(10.5f * scale)
                                            .LineHeight(1.7f));

                                        text.Justify();
                                        text.Span("توضیحات: ").Bold();
                                        text.Span(answer.Description);
                                    });
                            }
                        }
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static void MetaValue(
        ColumnDescriptor column,
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
                    .FontSize(10.2f * scale)
                    .FontColor(Colors.Black));

                text.Span(string.IsNullOrWhiteSpace(value) ? "-" : value);
            });
    }
}
