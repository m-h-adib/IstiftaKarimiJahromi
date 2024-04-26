using Application.Services.bases.Commands;
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
	private readonly IGetQuestionCategoriesService _getQuestionCategoriesService;
	private readonly IAddCountryService _addCountryService;
	private readonly IAddJobCategoryService _addJobCategoryService;
	private readonly IAddLanguageService _addLanguageService;
	private readonly IAddMethodOfReceiveQuestionService _addMethodOfReceiveQuestionService;
	private readonly IAddProvinceService _addProvinceService;
	private readonly IAddQuestionCategoryService _addQuestionCategoryService;
	private readonly IUpdateCountryService _updateCountryService;
	private readonly IUpdateJobCategoryService _updateJobCategoryService;
	private readonly IUpdateLanguageService _updateLanguageService;
	private readonly IUpdateMethodOfReceiveQuestionService _updateMethodOfReceiveQuestionService;
	private readonly IUpdateProvinceService _updateProvinceService;
	private readonly IUpdateQuestionCategoryService _updateQuestionCategoryService;
	private readonly IDeleteCountryService _deleteCountryService;
	private readonly IDeleteJobCategoryService _deleteJobCategoryService;
	private readonly IDeleteLanguageService _deleteLanguageService;
	private readonly IDeleteMethodOfReceiveQuestionService _deleteMethodOfReceiveQuestionService;
	private readonly IDeleteProvinceService _deleteProvinceService;
	private readonly IDeleteQuestionCategoryService _deleteQuestionCategoryService;
	private readonly IGetOneJobCategoryService _getOneJobCategoryService;
	private readonly IGetOneCountryService _getOneCountryService;
	private readonly IGetOneLanguageService _getOneLanguageService;
	private readonly IGetOneMethodOfRecieveQuestionService _getOneMethodOfRecieveQuestionService;
	private readonly IGetOneProvinceService _getOneProvinceService;
	#endregion
	#region ctors
	public BaseInfosController(IGetLanguagesService getLanguagesService,
		IGetJobCategoriesService getJobCategoriesService,
		IGetCountriesService getCountriesService,
		IGetMethodOfRecieveQuestionsService getMethodOfRecieveQuestionsService,
		IGetProvincesService getProvincesService,
		IGetQuestionCategoriesService getQuestionCategoriesService,
		IAddCountryService addCountryService,
		IAddJobCategoryService addJobCategoryService,
		IAddLanguageService addLanguageService,
		IAddMethodOfReceiveQuestionService addMethodOfReceiveQuestionService,
		IAddProvinceService addProvinceService,
		IAddQuestionCategoryService addQuestionCategoryService,
		IUpdateCountryService updateCountryService,
		IUpdateJobCategoryService updateJobCategoryService,
		IUpdateLanguageService updateLanguageService,
		IUpdateMethodOfReceiveQuestionService updateMethodOfReceiveQuestionService,
		IUpdateProvinceService updateProvinceService,
		IUpdateQuestionCategoryService updateQuestionCategoryService,
		IDeleteCountryService deleteCountryService,
		IDeleteJobCategoryService deleteJobCategoryService,
		IDeleteLanguageService deleteLanguageService,
		IDeleteMethodOfReceiveQuestionService deleteMethodOfReceiveQuestionService,
		IDeleteProvinceService deleteProvinceService,
		IDeleteQuestionCategoryService deleteQuestionCategoryService,
		IGetOneCountryService getOneCountryService,
		IGetOneJobCategoryService getOneJobCategoryService,
		IGetOneLanguageService getOneLanguageService,
		IGetOneMethodOfRecieveQuestionService getOneMethodOfRecieveQuestionService,
		IGetOneProvinceService getOneProvinceService)
    {
        _getCountriesService = getCountriesService;
        _getJobCategoriesService = getJobCategoriesService;
        _getLanguagesService = getLanguagesService;
        _getMethodOfRecieveQuestionsService = getMethodOfRecieveQuestionsService;
        _getProvincesService = getProvincesService;
		_getQuestionCategoriesService = getQuestionCategoriesService;
		_addCountryService = addCountryService;	
		_addJobCategoryService = addJobCategoryService;
		_addLanguageService = addLanguageService;
		_addMethodOfReceiveQuestionService = addMethodOfReceiveQuestionService;
		_addProvinceService	= addProvinceService;
		_addQuestionCategoryService = addQuestionCategoryService;
		_updateCountryService = updateCountryService;
		_updateJobCategoryService = updateJobCategoryService;
		_updateLanguageService = updateLanguageService;
		_updateMethodOfReceiveQuestionService = updateMethodOfReceiveQuestionService;
		_updateProvinceService = updateProvinceService;
		_updateQuestionCategoryService = updateQuestionCategoryService;
		_deleteCountryService = deleteCountryService;
		_deleteJobCategoryService = deleteJobCategoryService;
		_deleteLanguageService = deleteLanguageService;
		_deleteMethodOfReceiveQuestionService = deleteMethodOfReceiveQuestionService;
		_deleteProvinceService = deleteProvinceService;
		_deleteQuestionCategoryService = deleteQuestionCategoryService;
		_getOneCountryService = getOneCountryService;
		_getOneJobCategoryService = getOneJobCategoryService;
		_getOneLanguageService = getOneLanguageService;
		_getOneMethodOfRecieveQuestionService = getOneMethodOfRecieveQuestionService;
		_getOneProvinceService = getOneProvinceService;
    }
    #endregion


    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        var res = await _getCountriesService.ExecuteAsync();
        return StatusCode(res.StatusCode, res);
    }

	[HttpGet]
	public async Task<IActionResult> GetOneCountry(int CountryId)
	{
		var res = await _getOneCountryService.ExecuteAsync(CountryId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetJobCategories()
	{
		var res = await _getJobCategoriesService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetQuestionCategories()
	{
		var res = await _getQuestionCategoriesService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetOneJobCategory(int JobCategoryId)
	{
		var res = await _getOneJobCategoryService.ExecuteAsync(JobCategoryId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetLanguages()
	{
		var res = await _getLanguagesService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetOneLanguage(int LanguageId)
	{
		var res = await _getOneLanguageService.ExecuteAsync(LanguageId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetMethodOfRecieveQuestions()
	{
		var res = await _getMethodOfRecieveQuestionsService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetOneMethodOfRecieveQuestion(int MethodId)
	{
		var res = await _getOneMethodOfRecieveQuestionService.ExecuteAsync(MethodId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetProvinces(long? CountryId)
	{
		var res = await _getProvincesService.ExecuteAsync(CountryId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetOneProvince(int ProvinceId)
	{
		var res = await _getOneProvinceService.ExecuteAsync(ProvinceId);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPost]
	public async Task<IActionResult> AddCountry([FromBody]AddCountryDto dto)
	{
		var res=await _addCountryService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddProvince([FromBody] AddProvinceDto dto)
	{
		var res = await _addProvinceService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddJobCategory([FromBody] AddJobCategoryDto dto)
	{
		var res = await _addJobCategoryService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddLanguage([FromBody] AddLanguageDto dto)
	{
		var res = await _addLanguageService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddMethodOfReceiveQuestion([FromBody] AddMethodOfReceiveQuestionDto dto)
	{
		var res = await _addMethodOfReceiveQuestionService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddQuestionCategory([FromBody] AddQuestionCategoryDto dto)
	{
		var res = await _addQuestionCategoryService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateCountry([FromBody] UpdateCountryDto dto)
	{
		var res = await _updateCountryService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateProvince([FromBody] UpdateProvinceDto dto)
	{
		var res = await _updateProvinceService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateLanguage([FromBody] UpdateLanguageDto dto)
	{
		var res = await _updateLanguageService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateJobCategory([FromBody] UpdateJobCategoryDto dto)
	{
		var res = await _updateJobCategoryService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateMethodOfReceiveQuestion([FromBody] UpdateMethodOfReceiveQuestionDto dto)
	{
		var res = await _updateMethodOfReceiveQuestionService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateQuestionCategory([FromBody] UpdateQuestionCategoryDto dto)
	{
		var res = await _updateQuestionCategoryService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpDelete]
	public async Task<IActionResult> DeleteCountry([FromQuery] long? CountryId)
	{
		var res = await _deleteCountryService.ExecuteAsync(CountryId);
		return StatusCode(res.StatusCode, res);
	}


	[HttpDelete]
	public async Task<IActionResult> DeleteProvince([FromQuery] long? ProvinceId)
	{
		var res = await _deleteProvinceService.ExecuteAsync(ProvinceId);
		return StatusCode(res.StatusCode, res);
	}


	[HttpDelete]
	public async Task<IActionResult> DeleteLanguage([FromQuery] long? LanguageId)
	{
		var res = await _deleteLanguageService.ExecuteAsync(LanguageId);
		return StatusCode(res.StatusCode, res);
	}


	[HttpDelete]
	public async Task<IActionResult> DeleteJobCategory([FromQuery] long? JobCategoryId)
	{
		var res = await _deleteJobCategoryService.ExecuteAsync(JobCategoryId);
		return StatusCode(res.StatusCode, res);
	}


	[HttpDelete]
	public async Task<IActionResult> DeleteMethodOfReceiveQuestion([FromQuery] long? MethodOfReceiveQuestionId)
	{
		var res = await _deleteMethodOfReceiveQuestionService.ExecuteAsync(MethodOfReceiveQuestionId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpDelete]
	public async Task<IActionResult> DeleteQuestionCategory([FromQuery] long? QuestionCategoryId)
	{
		var res = await _deleteQuestionCategoryService.ExecuteAsync(QuestionCategoryId);
		return StatusCode(res.StatusCode, res);
	}
}
