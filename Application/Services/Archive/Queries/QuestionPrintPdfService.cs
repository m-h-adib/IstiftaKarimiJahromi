using System.Data;
using FastReport;
using FastReport.Export.PdfSimple;
using FastReport.Utils;

namespace Application.Services.Archive.Queries;

public interface IQuestionPrintPdfService
{
    byte[] Generate(GetQuestionPrintDto question, string format, string reportsDirectory);
}

public class QuestionPrintPdfService : IQuestionPrintPdfService
{
    private static readonly object FontLock = new();
    private static bool _fontReady;
    public byte[] Generate(
        GetQuestionPrintDto question,
        string format,
        string reportsDirectory)
    {
        var reportFileName = string.Equals(format, "A4", StringComparison.OrdinalIgnoreCase)
            ? "EstefTa-A4.frx"
            : "EstefTa-A5.frx";

        var reportPath = Path.Combine(reportsDirectory, reportFileName);

        if (!File.Exists(reportPath))
            throw new FileNotFoundException("قالب گزارش استفتاء یافت نشد.", reportPath);

        var fontPath = Path.Combine(
            Directory.GetParent(reportsDirectory)?.FullName ?? reportsDirectory,
            "Fonts",
            "BNAZANIN.TTF");

        if (!File.Exists(fontPath))
            throw new FileNotFoundException("فونت B Nazanin یافت نشد.", fontPath);

        EnsureFontRegistered(fontPath);

        using var report = new FastReport.Report();

        report.Load(reportPath);

        report.SetParameterValue("QuestionId", question.Id.ToString("D8"));
        report.SetParameterValue("QuestionDate", question.QuestionDate ?? string.Empty);
        report.SetParameterValue("Name", question.Name ?? string.Empty);
        report.SetParameterValue("QuestionText", question.QuestionText ?? string.Empty);

        var dataSet = BuildDataSet(question);
        report.RegisterData(dataSet, "Istifta", true);

        if (!report.Prepare())
            throw new InvalidOperationException("تهیه گزارش استفتاء با خطا مواجه شد.");

        using var stream = new MemoryStream();
        using var pdfExport = new PDFSimpleExport
        {
            ImageDpi = 300,
            JpegQuality = 95,
            Title = $"استفتاء {question.Id:D8}",
            Subject = "نسخه چاپی استفتاء"
        };

        report.Export(pdfExport, stream);

        return stream.ToArray();
    }

    private static void EnsureFontRegistered(string fontPath)
    {
        if (_fontReady)
            return;

        lock (FontLock)
        {
            if (_fontReady)
                return;

            FontManager.AddFont(fontPath);

            var nazaninFamily = FontManager.AllFamilies
                .FirstOrDefault(f =>
                    string.Equals(f.Name, "B Nazanin", StringComparison.OrdinalIgnoreCase) ||
                    f.Name.Contains("Nazanin", StringComparison.OrdinalIgnoreCase));

            if (nazaninFamily == null)
            {
                throw new InvalidOperationException(
                    $"فونت B Nazanin در FastReport ثبت نشد. مسیر فونت: {fontPath}");
            }

            _fontReady = true;
        }
    }

    private static DataSet BuildDataSet(GetQuestionPrintDto question)
    {
        var dataSet = new DataSet("Istifta");

        var answers = new DataTable("Answers");
        answers.Columns.Add("Language", typeof(string));
        answers.Columns.Add("Answer", typeof(string));
        answers.Columns.Add("Reason", typeof(string));
        answers.Columns.Add("Description", typeof(string));

        foreach (var answer in question.Answers)
        {
            answers.Rows.Add(
                answer.Language ?? string.Empty,
                answer.Answer ?? string.Empty,
                answer.Reason ?? string.Empty,
                answer.Description ?? string.Empty);
        }

        dataSet.Tables.Add(answers);

        return dataSet;
    }
}
