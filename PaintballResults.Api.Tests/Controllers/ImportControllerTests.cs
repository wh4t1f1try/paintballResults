using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Paintball.Abstractions.Services;
using PaintballResults.Api.Controllers;

[TestClass]
public class ImportControllerTests
{
    private ImportController _importController;
    private IImportService _importService;

    [TestInitialize]
    public void Setup()
    {
        this._importService = Substitute.For<IImportService>();
        this._importController = new ImportController(this._importService);
    }

    [TestMethod]
    public async Task ImportGameResultCsv_ShouldReturnOkResult_WhenFileIsImported()
    {
        // Arrange
        IFormFile? fileMock = Substitute.For<IFormFile>();
        MemoryStream sourceStream = new MemoryStream();
        fileMock.OpenReadStream().Returns(sourceStream);
        // Act
        IActionResult result = await this._importController.ImportGameResultCsv(fileMock);
        // Assert
        this._importService.Received(1).ImportGameResults(Arg.Any<Stream>());
        OkObjectResult? okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be("Import erfolgreich.");
    }
}