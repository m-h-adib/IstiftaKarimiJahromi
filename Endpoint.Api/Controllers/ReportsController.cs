using Application.Services.Report.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReportsController : ControllerBase
{
    #region members
    private readonly IGetUserQueryService _userQueryService;
    #endregion
    #region ctors
    public ReportsController(IGetUserQueryService userQueryService)
    {
        _userQueryService = userQueryService;
    }
    #endregion


    [HttpGet]
    public async Task<IActionResult> GetUserQuery([FromQuery]GetUserQueryRequestDto dto)
    {
        var res=await _userQueryService.ExecuteAsync(dto);
        return StatusCode(res.StatusCode, res);
    }
}
