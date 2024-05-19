using Application.Services.Process.Quesries;
using Application.Services.Question.Commands;
using Application.Services.Question.Quesries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class QuestionsController : ControllerBase
	{
		#region members
		private readonly IGetQuestionsService _getQuestionsService;
		private readonly IGetOneQuestionService _getOneQuestionService;
		private readonly IAddQuestionService _addQuestionService;
		private readonly IEditQuestionService _editQuestionService;
		private readonly IDeleteQuestionService _deleteQuestionService;
		private readonly IDeleteQuestionFileService _deleteQuestionFileService;
		private readonly IReferenceToProcessorService _referenceToProcessorService;
		#endregion
		#region ctors
		public QuestionsController(IGetQuestionsService getQuestionsService,
			IGetOneQuestionService getOneQuestionService,
			IAddQuestionService addQuestionService,
			IEditQuestionService editQuestionService,
			IDeleteQuestionService deleteQuestionService,
			IDeleteQuestionFileService deleteQuestionFileService,
			IReferenceToProcessorService referenceToProcessorService)
        {
            _addQuestionService = addQuestionService;
			_deleteQuestionFileService = deleteQuestionFileService;
			_deleteQuestionService	= deleteQuestionService;
			_editQuestionService = editQuestionService;
			_getQuestionsService = getQuestionsService;
			_getOneQuestionService = getOneQuestionService;
			_referenceToProcessorService = referenceToProcessorService;

		}
		#endregion


	

		[HttpGet]
		public async Task<IActionResult> GetQuestions([FromQuery] GetQuestionRequestDto dto)
		{
			var res = await _getQuestionsService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}


		[HttpGet]
		public async Task<IActionResult> GetOneQuestion([FromQuery] long? QuestionId)
		{
			var res = await _getOneQuestionService.ExecuteAsync(QuestionId);
			return StatusCode(res.StatusCode, res);
		}


		[HttpPost]
		public async Task<IActionResult> AddQuestion([FromForm] AddQuestionDto dto)
		{
			var res = await _addQuestionService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}

		[HttpPost]
		public async Task<IActionResult> ReferenceToProcessor([FromBody] ReferenceToProcessorDto dto)
		{
			var res = await _referenceToProcessorService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}


		[HttpPut]
		public async Task<IActionResult> EditQuestion([FromForm] EditQuestionDto dto)
		{
			var res = await _editQuestionService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteQuestion([FromQuery] long? QuestionId)
		{
			var res = await _deleteQuestionService.ExecuteAsync(QuestionId);
			return StatusCode(res.StatusCode, res);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteQuestionFile([FromQuery] long? QuestionFileId)
		{
			var res = await _deleteQuestionFileService.ExecuteAsync(QuestionFileId);
			return StatusCode(res.StatusCode, res);
		}
	}
}
