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
        private readonly IGetArchiveQuestionService _getArchiveQuestionService;
        private readonly IGetQuestionStepsService _getQuestionStepsService;
        private readonly IGetQuestionResultService _getQuestionResultService;
        private readonly IDeleteArchiveQuestionService _deleteArchiveQuestionService;
        private readonly IGetQuestionPrintService _getQuestionPrintService;
		private readonly ISendAnswerDateService _sendAnswerDateService;
		public ArchiveController(IGetArchiveQuestionService getArchiveQuestionService,
            IGetQuestionResultService getQuestionResultService,
            IGetQuestionStepsService getQuestionStepsService,
            IDeleteArchiveQuestionService deleteArchiveQuestionService,
			ISendAnswerDateService sendAnswerDateService,
            IGetQuestionPrintService getQuestionPrintService)
        {
            _getArchiveQuestionService = getArchiveQuestionService;
            _getQuestionResultService = getQuestionResultService;
            _getQuestionStepsService = getQuestionStepsService;
			_sendAnswerDateService = sendAnswerDateService;
			_deleteArchiveQuestionService = deleteArchiveQuestionService;
            _getQuestionPrintService = getQuestionPrintService;
        }


        [HttpGet]
        public async Task<IActionResult> GetArchiveQuestions([FromQuery]GetArchiveQuestionRequestDto dto)
        {
            var res=await _getArchiveQuestionService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionSteps([FromQuery] long? QuestionId)
        {
            var res = await _getQuestionStepsService.ExecuteAsync(QuestionId);
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionResults([FromQuery]long? QuestionId)
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
