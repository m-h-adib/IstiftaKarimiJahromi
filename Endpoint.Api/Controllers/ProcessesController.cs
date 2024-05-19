using Application.Services.Process.Commands;
using Application.Services.Process.Queries;
using Application.Services.Process.Quesries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ProcessesController : ControllerBase
{
	#region members
	private readonly IGetProcessQuestionsService _processQuestionsService;
    private readonly IGetProcessCyclesService _processCyclesService;
    private readonly ISearchQuestionService _searchQuestionService;
    private readonly IAddProcessQuestionService _addProcessQuestionService;
    #endregion
    #region ctors
    public ProcessesController(IGetProcessQuestionsService processQuestionsService,
        IGetProcessCyclesService processCyclesService,
        ISearchQuestionService searchQuestionService,
        IAddProcessQuestionService addProcessQuestionService)
    {
        _processQuestionsService = processQuestionsService;
        _addProcessQuestionService = addProcessQuestionService;
        _processCyclesService = processCyclesService;
        _searchQuestionService = searchQuestionService;
    }
    #endregion


    [HttpGet]
    public async Task<IActionResult> GetQuestions([FromQuery]GetProcessQuestionRequestDto dto)
    {
        var res=await _processQuestionsService.ExecuteAsync(dto);
        return StatusCode(res.StatusCode, res);
    }


	[HttpGet]
	public async Task<IActionResult> GetCycles([FromQuery] GetProcessCycleRequestDto dto)
	{
		var res = await _processCyclesService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> SearchQuestion([FromQuery] SearchQuestionRequestDto dto)
	{
		var res = await _searchQuestionService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddProcessQuestion([FromBody] AddProcessQuestionDto dto)
	{
		var res = await _addProcessQuestionService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}
}
