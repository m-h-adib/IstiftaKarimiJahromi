using Application.AdminServices.User.Queries;
using Application.Services.Person.Commands;
using Application.Services.User.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
	#region members
	private readonly IAddUserService _addUserService;
	private readonly IDeleteUserService _deleteUserService;
	private readonly IUpdateUserAccessService _updateUserAccessService;
	private readonly IGetUsersService _getUsersService;
	private readonly IGetUserAccessService _getUserAccessService;
    #endregion
    #region ctors
    public UsersController(IGetUserAccessService getUserAccessService,
        IGetUsersService getUsersService,
        IAddUserService addUserService,
        IUpdateUserAccessService updateUserAccessService,
        IDeleteUserService deleteUserService)
    {
        _addUserService = addUserService;
        _deleteUserService = deleteUserService;
        _getUserAccessService = getUserAccessService;
        _getUsersService = getUsersService;
        _updateUserAccessService = updateUserAccessService;
    }
	#endregion


	[HttpGet]
	public async Task<IActionResult> GetUsers([FromQuery] GetAminsRequestDto dto)
	{
		var res = await _getUsersService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpGet]
	public async Task<IActionResult> GetUserAccess(int UserId)
	{
		var res = await _getUserAccessService.ExecuteAsync(UserId);
		return StatusCode(res.StatusCode, res);
	}

	[HttpPost]
	public async Task<IActionResult> AddUser([FromBody] AddAdminDto dto)
	{
		var res = await _addUserService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}


	[HttpPut]
	public async Task<IActionResult> UpdateUserAccess([FromBody] UpdateUserAccessDto dto)
	{
		var res = await _updateUserAccessService.ExecuteAsync(dto);
		return StatusCode(res.StatusCode, res);
	}

	[HttpDelete]
	public async Task<IActionResult> DeleteUser(int UserId)
	{
		var res = await _deleteUserService.ExecuteAsync(UserId);
		return StatusCode(res.StatusCode, res);
	}
}
