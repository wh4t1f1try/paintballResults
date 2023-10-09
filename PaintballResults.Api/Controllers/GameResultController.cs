using Microsoft.AspNetCore.Mvc;
using Paintball.Abstractions.Services;
using Paintball.Database.Abstractions.Entities;

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
            IList<GameResult> gameResults = this.GameResultService.GetAll();
            return this.Ok(gameResults);
        }


        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetGameResultsById([FromRoute] int id)
        {
            GameResult gameResult = this.GameResultService.GetById(id);
            return this.Ok(gameResult);
        }


        [HttpGet("team/{team}")]
        public async Task<IActionResult> GetAllResultsFromTeam(string team)
        {
            IList<GameResult> gameResults = this.GameResultService.GetByName(team);
            return this.Ok(gameResults);
        }


        private IGameResultService GameResultService { get; }
    }
}