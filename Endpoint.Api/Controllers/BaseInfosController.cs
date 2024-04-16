using Application.Services.bases.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class BaseInfosController : ControllerBase
{
	#region members
	private readonly IGetMethodOfRecieveQuestionsService _getMethodOfRecieveQuestionsService;
	private readonly IGetProvincesService _getProvincesService;
	private readonly IGetCountriesService _getCountriesService;
	private readonly IGetJobCategoriesService _getJobCategoriesService;
	private readonly IGetLanguagesService _getLanguagesService;
    #endregion
    #region ctors
    public BaseInfosController(IGetLanguagesService getLanguagesService,
        IGetJobCategoriesService getJobCategoriesService,
        IGetCountriesService getCountriesService,
        IGetMethodOfRecieveQuestionsService getMethodOfRecieveQuestionsService,
        IGetProvincesService getProvincesService)
    {
        _getCountriesService = getCountriesService;
        _getJobCategoriesService = getJobCategoriesService;
        _getLanguagesService = getLanguagesService;
        _getMethodOfRecieveQuestionsService = getMethodOfRecieveQuestionsService;
        _getProvincesService = getProvincesService;
    }
    #endregion


    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        var res = await _getCountriesService.ExecuteAsync();
        return StatusCode(res.StatusCode, res);
    }

	[HttpGet]
	public async Task<IActionResult> GetJobCategories()
	{
		var res = await _getJobCategoriesService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetLanguages()
	{
		var res = await _getLanguagesService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetMethodOfRecieveQuestions()
	{
		var res = await _getMethodOfRecieveQuestionsService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetProvinces(long? CountryId)
	{
		var res = await _getProvincesService.ExecuteAsync(CountryId);
		return StatusCode(res.StatusCode, res);
	}
}
