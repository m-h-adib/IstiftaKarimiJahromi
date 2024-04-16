using Application.Services.Authentication.Commands;
using Application.Services.Authentication.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace Endpoint.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class IdentityController : ControllerBase
{
	#region members
	private readonly ILoginService _loginService;
    private readonly IGetAccessService _getAccessService;
    private readonly IGetMenuAccessService _getMenuAccessService;
    private readonly IGetOneAccessService _getOneAccessService;
    #endregion
    #region ctors
    public IdentityController(ILoginService loginService,
        IGetAccessService getAccessService,
        IGetMenuAccessService getMenuAccessService,
        IGetOneAccessService getOneAccessService)
    {
        _loginService = loginService;
        _getAccessService = getAccessService;
        _getMenuAccessService = getMenuAccessService;
        _getOneAccessService = getOneAccessService;
    }
	#endregion



	[HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAccess()
    {
        var res = await _getAccessService.ExecuteAsync();
        return StatusCode(res.StatusCode, res);
    }


	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetMunuAccess()
	{
		var res = await _getMenuAccessService.ExecuteAsync();
		return StatusCode(res.StatusCode, res);
	}


	[HttpGet]
	[Authorize]
	public async Task<IActionResult> GetOneAccess(int SectionId)
	{
		var res = await _getOneAccessService.ExecuteAsync(SectionId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var res = await _loginService.ExecuteAsync(dto);
        return StatusCode(res.StatusCode, res);
    }


    

}
