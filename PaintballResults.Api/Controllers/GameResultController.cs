using Microsoft.AspNetCore.Mvc;
using Paintball.Abstractions.Services;


namespace PaintballResults.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameResultController : ControllerBase
    {
        private IGameResultService GameResultService { get; }

        public GameResultController
        (
            IGameResultService gameResultService
        )
        {
            GameResultService = gameResultService;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllGameResults()
        {
            var gameResults = GameResultService.GetAll();
            return Ok(gameResults);
        }


        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetGameResultsById([FromRoute] int id)
        {
            var gameResult = GameResultService.GetById(id);
            return Ok(gameResult);
        }


        [HttpGet("team/{team}")]
        public async Task<IActionResult> GetAllResultsFromTeam(string team)
        {
            var gameResults = GameResultService.GetByName(team);
            return Ok(gameResults);
        }

        //todo: Zum testen, löschen vor Merge!
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAllResultsFromDatabase()
        {
            GameResultService.Delete();
            return Ok("Deleted.");
        }
    }
}