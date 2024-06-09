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
    public class ResearchesController : ControllerBase
    {
        #region members
        private readonly IGetResearchQuestionsService _researchQuestionsService;
        private readonly IGetResearchCyclesService _researchCyclesService;
        private readonly IAddResearchQuestionService _addResearchQuestionService;
        private readonly IAcceptOrRejectResearchService _acceptOrRejectResearchService;
        #endregion
        #region ctors
        public ResearchesController(IGetResearchQuestionsService getResearchQuestionsService,
            IGetResearchCyclesService getResearchCyclesService,
            IAddResearchQuestionService addResearchQuestionService,
            IAcceptOrRejectResearchService acceptOrRejectResearchService)
        {
            _researchCyclesService = getResearchCyclesService;
            _researchQuestionsService = getResearchQuestionsService;
            _addResearchQuestionService = addResearchQuestionService;
            _acceptOrRejectResearchService = acceptOrRejectResearchService;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> GetResearchQuestions([FromQuery] GetResearchQuestionRequestDto dto)
        {
            var res = await _researchQuestionsService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }


        [HttpGet]
        public async Task<IActionResult> GetRersearchCycles([FromQuery] GetResearchCycleRequestDto dto)
        {
            var res =await _researchCyclesService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }


        [HttpPost]
        public async Task<IActionResult> AddResearchQuestion([FromBody] AddResearchQuestionDto dto)
        {
            var res=await  _addResearchQuestionService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrRejectResearch([FromBody] AcceptOrRejectResearchDto dto)
        {
            var res=await _acceptOrRejectResearchService.ExecuteAsync(dto);
            return StatusCode(res.StatusCode, res);
        }
    }
}
