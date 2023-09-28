using Microsoft.AspNetCore.Mvc;
using Paintball.Abstractions.Services;


namespace PaintballResults.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class ImportController : ControllerBase

    {
        private IImportService ImportService { get; }

        public ImportController(IImportService importService)
        {
            ImportService = importService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportGameResultCsv(IFormFile file)
        {
            ImportService.ImportGameResults(file.OpenReadStream());
            return Ok("Import erfolgreich.");
        }
    }
}