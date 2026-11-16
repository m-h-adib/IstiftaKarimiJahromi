using Application.Services.Archive.Commands;
using Application.Services.Archive.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ArchiveController : ControllerBase
    {
        private readonly IGetArchiveQuestionService _getArchiveQuestionService;\n        private readonly IExportArchiveQuestionsExcelService _exportArchiveQuestionsExcelService;
        private readonly IGetQuestionStepsService _getQuestionStepsService;
        private readonly IGetQuestionResultService _getQuestionResultService;
        private readonly IDeleteArchiveQuestionService _deleteArchiveQuestionService;
        private readonly IGetQuestionPrintService _getQuestionPrintService;
        private readonly IQuestionPrintPdfService _questionPrintPdfService;
        private readonly ISendAnswerDateService _sendAnswerDateService;

        public ArchiveController(
            IGetArchiveQuestionService getArchiveQuestionService,\n            IExportArchiveQuestionsExcelService exportArchiveQuestionsExcelService,
            IGetQuestionResultService getQuestionResultService,
            IGetQuestionStepsService getQuestionStepsService,
            IDeleteArchiveQuestionService deleteArchiveQuestionService,
            ISendAnswerDateService sendAnswerDateService,
            IGetQuestionPrintService getQuestionPrintService,
            IQuestionPrintPdfService questionPrintPdfService)
        {
            _getArchiveQuestionService = getArchiveQuestionService;\n            _exportArchiveQuestionsExcelService = exportArchiveQuestionsExcelService;
            _getQuestionResultService = getQuestionResultService;
            _getQuestionStepsService = getQuestionStepsService;
            _sendAnswerDateService = sendAnswerDateService;
            _deleteArchiveQuestionService = deleteArchiveQuestionService;
            _getQuestionPrintService = getQuestionPrintService;
            _questionPrintPdfService = questionPrintPdfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetArchiveQuestions([FromQuery] GetArchiveQuestionRequestDto dto)
        {
            var res = await _getArchiveQuestionService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> ExportArchiveQuestionsExcel([FromQuery] GetArchiveQuestionRequestDto dto)
        {
            var result = await _exportArchiveQuestionsExcelService.ExecuteAsync(dto);
            if (!result.IsSuccess || result.Data == null)
                return StatusCode(result.StatusCode, result);

            var fileName = $"ArchiveQuestions-{DateTime.Now:yyyyMMdd-HHmmss}.xlsx";
            return File(result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionSteps([FromQuery] long? QuestionId)
        {
            var res = await _getQuestionStepsService.ExecuteAsync(QuestionId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionResults([FromQuery] long? QuestionId)
        {
            var res = await _getQuestionResultService.ExecuteAsync(QuestionId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionPrint([FromQuery] long? QuestionId)
        {
            var res = await _getQuestionPrintService.ExecuteAsync(QuestionId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionPrintPdf(
            [FromQuery] long? QuestionId,
            [FromQuery] string format = "A5")
        {
            var res = await _getQuestionPrintService.ExecuteAsync(QuestionId);

            if (!res.IsSuccess || res.Data == null)
                return StatusCode(res.StatusCode, res);

            if (!string.Equals(format, "A4", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(format, "A5", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("فرمت چاپ باید A4 یا A5 باشد.");
            }

            var reportsDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Reports");

            var pdf = _questionPrintPdfService.Generate(
                res.Data,
                format,
                reportsDirectory);

            return File(
                pdf,
                "application/pdf",
                $"istifta-{res.Data.Id:D8}-{format.ToUpperInvariant()}.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> SendAnswerDate([FromBody] SendAnswerDateDto dto)
        {
            var res = await _sendAnswerDateService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteArchiveQuestion([FromQuery] long? QuestionId)
        {
            var res = await _deleteArchiveQuestionService.ExecuteAsync(QuestionId);
            return StatusCode(res.StatusCode, res);
        }
    }
}
