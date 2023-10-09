using Microsoft.AspNetCore.Mvc;
using Paintball.Abstractions.Services;

namespace PaintballResults.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class ImportController : ControllerBase

    {
        public ImportController(IImportService importService)
        {
            this.ImportService = importService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportGameResultCsv(IFormFile file)
        {
            this.ImportService.ImportGameResults(file.OpenReadStream());
            return this.Ok("Import erfolgreich.");
        }

        private IImportService ImportService { get; }
    }
}