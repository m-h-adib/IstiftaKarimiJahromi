using Application.Services.Session.Commands;
using Application.Services.Session.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.Services.Session.Commands.UpdateSessionService;

namespace Endpoint.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class SessionsController : ControllerBase
{
    #region members
    private readonly IGetSessionsService _getSessionsService;
    private readonly IGetOneSessionService _getOneSessionService;
    private readonly IAddSessionService _addSessionService;
    private readonly IDeleteSessionService _deleteSessionService;
    private readonly IUpdateSessionService _updateSessionService;
    #endregion
    #region ctors
    public SessionsController(IGetSessionsService getSessionsService,
        IGetOneSessionService getOneSessionService,
        IAddSessionService addSessionService,
        IUpdateSessionService updateSessionService,
        IDeleteSessionService deleteSessionService)
    {
        _addSessionService = addSessionService;
        _getSessionsService = getSessionsService;
        _getOneSessionService = getOneSessionService;
        _updateSessionService = updateSessionService;
        _deleteSessionService = deleteSessionService;
    }
    #endregion


    [HttpGet]
    public async Task<IActionResult> GetSessions([FromQuery]GetSessionRequestDto dto)
    {
        var res = await _getSessionsService.ExecuteAsync(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetOneSession([FromQuery]long Id)
    {
        var res = await _getOneSessionService.ExecuteAsync(Id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost]
    public async Task<IActionResult> Addsession([FromBody]AddSessionDto dto)
    {
        var res = await _addSessionService.ExecuteAsync(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSession([FromBody] UpdateSessionDto dto)
    {
        var res = await _updateSessionService.ExecuteAsync(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSession([FromQuery]long Id)
    {
        var res = await _deleteSessionService.ExecuteAsync(Id);
        return StatusCode(res.StatusCode, res);
    }
}
