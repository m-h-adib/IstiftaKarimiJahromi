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

        var document = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(pageSize.Portrait());
                page.MarginHorizontal(isA4 ? 40 : 28);
                page.MarginTop(isA4 ? 18 : 14);
                page.MarginBottom(isA4 ? 22 : 18);
                page.PageColor(Colors.White);
                page.ContentFromRightToLeft();

                page.DefaultTextStyle(x => x
                    .FontFamily("Tahoma", "Arial")
                    .FontSize(isA4 ? 10 : 8.5f)
                    .FontColor(Colors.Black));

                page.Header().Column(header =>
                {
                    header.Item()
                        .Background("#C8A16E")
                        .Height(isA4 ? 32 : 25)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text("واحد پاسخ به مسائل شرعی")
                        .Bold()
                        .FontSize(isA4 ? 13 : 10.5f);

                    header.Item()
                        .PaddingTop(isA4 ? 8 : 6)
                        .PaddingBottom(isA4 ? 7 : 5)
                        .Row(row =>
                        {
                            row.RelativeItem().AlignRight().Column(meta =>
                            {
                                MetaRow(meta, "شماره استفتاء", question.Id.ToString("D8"));
                                MetaRow(meta, "تاریخ وصول", question.QuestionDate);
                                MetaRow(meta, "نام و نام خانوادگی", question.Name);
                            });

                            row.ConstantItem(isA4 ? 175 : 125)
                                .AlignRight()
                                .Column(logo =>
                                {
                                    logo.Item()
                                        .AlignRight()
                                        .Width(isA4 ? 150 : 108)
                                        .Height(isA4 ? 42 : 31)
                                        .Background("#006B3C")
                                        .CornerRadius(9)
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text("بِسْمِ اللَّهِ الرَّحْمَنِ الرَّحِيمِ")
                                        .FontColor("#C99B57")
                                        .FontSize(isA4 ? 9 : 6.5f)
                                        .Bold();

                                    logo.Item()
                                        .PaddingTop(3)
                                        .AlignRight()
                                        .Text("✺")
                                        .FontColor("#006B3C")
                                        .FontSize(isA4 ? 16 : 12);
                                });
                        });

                    header.Item()
                        .BorderBottom(0.8f)
                        .BorderColor("#C8A16E");
                });

                page.Content().PaddingTop(10).Column(column =>
                {
                    column.Spacing(isA4 ? 12 : 8);

                    column.Item().Column(section =>
                    {
                        SectionTitle(section, "متن استفتاء");

                        section.Item()
                            .PaddingTop(5)
                            .Text(question.QuestionText ?? "-")
                            .LineHeight(1.7f)
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
                                    .PaddingTop(5)
                                    .Column(answerColumn =>
                                    {
                                        answerColumn.Item()
                                            .Text(answer.Answer ?? "-")
                                            .LineHeight(1.7f)
                                            .Justify();

                                        if (!string.IsNullOrWhiteSpace(answer.Reason))
                                        {
                                            answerColumn.Item()
                                                .PaddingTop(5)
                                                .Text(text =>
                                                {
                                                    text.Span("دلیل: ").Bold();
                                                    text.Span(answer.Reason);
                                                })
                                                .LineHeight(1.6f)
                                                .Justify();
                                        }

                                        if (!string.IsNullOrWhiteSpace(answer.Description))
                                        {
                                            answerColumn.Item()
                                                .PaddingTop(5)
                                                .Text(text =>
                                                {
                                                    text.Span("توضیحات: ").Bold();
                                                    text.Span(answer.Description);
                                                })
                                                .LineHeight(1.6f)
                                                .Justify();
                                        }
                                    });
                            }
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(stampSvg))
                    {
                        column.Item()
                            .PaddingTop(12)
                            .AlignLeft()
                            .Width(isA4 ? 90 : 70)
                            .Svg(stampSvg)
                            .FitWidth();
                    }
                });

                page.Footer()
                    .PaddingTop(5)
                    .BorderTop(0.5f)
                    .BorderColor("#C8A16E")
                    .AlignCenter()
                    .Text("karimijahromi.ir")
                    .FontColor("#006B3C")
                    .FontSize(isA4 ? 8 : 6.5f);
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
            .BorderColor("#C8A16E")
            .PaddingBottom(3)
            .Text(title)
            .Bold()
            .FontSize(10.5f);
    }
}
