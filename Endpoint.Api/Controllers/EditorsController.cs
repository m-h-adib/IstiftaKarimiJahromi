using Application.Services.Archive.Queries;
using Application.Services.Editor.Commands;
using Application.Services.Editor.Queries;
using Application.Services.Research.Commands;
using Application.Services.Research.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class EditorsController : ControllerBase
	{
		#region members
		private readonly IGetEditorQuestionsService _editorQuestionsService;
		private readonly IGetEditorCyclesService _editorCyclesService;
		private readonly IAddEditorQuestionService _addEditorQuestionService;
		private readonly IAcceptOrRejectEditorService _acceptOrRejectEditorService;
		
		#endregion
		#region ctors
		public EditorsController(IGetEditorQuestionsService getEditorQuestionsService,
			IGetEditorCyclesService getEditorCyclesService,
			IAddEditorQuestionService addEditorQuestionService,
			IAcceptOrRejectEditorService acceptOrRejectEditorService)
		{
			_editorCyclesService = getEditorCyclesService;
			_editorQuestionsService = getEditorQuestionsService;
			_addEditorQuestionService = addEditorQuestionService;
			_acceptOrRejectEditorService = acceptOrRejectEditorService;
		}
		#endregion


		[HttpGet]
		public async Task<IActionResult> GetEditorQuestions([FromQuery] GetEditorQuestionRequestDto dto)
		{
			var res = await _editorQuestionsService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}


		[HttpGet]
		public async Task<IActionResult> GetEditorCycles([FromQuery] GetEditorCycleRequestDto dto)
		{
			var res = await _editorCyclesService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}


		[HttpPost]
		public async Task<IActionResult> AddEditorQuestion([FromBody] AddEditorQuestionDto dto)
		{
			var res = await _addEditorQuestionService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}

		[HttpPost]
		public async Task<IActionResult> AcceptOrRejectEditor([FromBody] AcceptOrRejectEditorDto dto)
		{
			var res = await _acceptOrRejectEditorService.ExecuteAsync(dto);
			return StatusCode(res.StatusCode, res);
		}

		
	}
}
