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
        public ArchiveController(IGetArchiveQuestionService getArchiveQuestionService,
            IGetQuestionResultService getQuestionResultService,
            IGetQuestionStepsService getQuestionStepsService,
            IDeleteArchiveQuestionService deleteArchiveQuestionService)
        {
            _getArchiveQuestionService = getArchiveQuestionService;
            _getQuestionResultService = getQuestionResultService;
            _getQuestionStepsService = getQuestionStepsService;
            _deleteArchiveQuestionService = deleteArchiveQuestionService;
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



        [HttpDelete]
        public async Task<IActionResult> DeleteArchiveQuestion([FromQuery] long? QuestionId)
        {
            var res = await _deleteArchiveQuestionService.ExecuteAsync(QuestionId);
            return StatusCode(res.StatusCode, res);
        }
    }
}
