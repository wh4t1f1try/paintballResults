#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PaintballResults.Api.Tests.Controllers;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Paintball.Abstractions.Services;
using PaintballResults.Api.Controllers;

[TestClass]
public class ImportControllerTests
{
    private ImportController importController;
    private IImportService importService;

    [TestInitialize]
    public void Setup()
    {
        this.importService = Substitute.For<IImportService>();
        this.importController = new ImportController(this.importService);
    }

    [TestMethod]
    public async Task ImportGameResultCsv_ShouldReturnOkResult_WhenFileIsImported()
    {
        // Arrange
        IFormFile? fileMock = Substitute.For<IFormFile>();
        MemoryStream sourceStream = new MemoryStream();
        fileMock.OpenReadStream().Returns(sourceStream);
        // Act
        IActionResult result = await this.importController.ImportGameResultCsv(fileMock);
        // Assert
        this.importService.Received(1).ImportGameResults(Arg.Any<Stream>());
        OkObjectResult? okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be("Import erfolgreich.");
    }
}