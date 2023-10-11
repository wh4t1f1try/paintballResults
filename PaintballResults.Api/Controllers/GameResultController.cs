using Microsoft.AspNetCore.Mvc;
using Paintball.Abstractions.DTOs;
using Paintball.Abstractions.Services;

namespace PaintballResults.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameResultController : ControllerBase
    {
        public GameResultController
        (
            IGameResultService gameResultService
        )
        {
            this.GameResultService = gameResultService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllGameResults()
        {
            IList<GameResultDto> gameResults = this.GameResultService.GetAll();
            return this.Ok(gameResults);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetGameResultsById([FromRoute] int id)
        {
            GameResultDto gameResult = this.GameResultService.GetById(id);
            return this.Ok(gameResult);
        }

        [HttpGet("team/{team}")]
        public async Task<IActionResult> GetAllResultsFromTeam(string team)
        {
            IList<GameResultDto> gameResults = this.GameResultService.GetByName(team);
            return this.Ok(gameResults);
        }

        //[HttpGet("all")]
        //public IList<GameResultDto> GetAllGameResults() => this.GameResultService.GetAll();

        private IGameResultService GameResultService { get; }
    }
}