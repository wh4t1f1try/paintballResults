namespace PaintballResults.Api.Tests.Controllers;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Paintball.Abstractions.Services;
using Paintball.Database.Abstractions.Entities;
using PaintballResults.Api.Controllers;

[TestClass]
public class GameResultControllerTests
{
    private GameResultController _controller;
    private IGameResultService _gameResultService;


    [TestInitialize]
    public void Setup()
    {
        this._gameResultService = Substitute.For<IGameResultService>();
        this._controller = new GameResultController(this._gameResultService);
    }

    [TestMethod]
    public async Task GetAllGameResults_ShouldReturnOk_WhenGameResultsExist()
    {
        //Arrange
        IList<GameResult> gameResults = new List<GameResult>
        {
            new(),
            new()
        };
        this._gameResultService.GetAll().Returns(gameResults);

        //Act
        IActionResult result = await this._controller.GetAllGameResults();

        //Assert
        OkObjectResult? okObjectResult = result as OkObjectResult;
        okObjectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okObjectResult.Value.Should().BeEquivalentTo(gameResults);
    }

    [TestMethod]
    public async Task GetAllGameResulst_ShouldReturnOkObjectResultWith200_WhenNoGameResultsExist()
    {
        //Arrange
        IList<GameResult> gameResults = new List<GameResult>();
        this._gameResultService.GetAll().Returns(gameResults);

        //Act
        IActionResult result = await this._controller.GetAllGameResults();

        //Assert
        OkObjectResult? okObjectResult = result as OkObjectResult;
        okObjectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okObjectResult.Value.Should().BeEquivalentTo(gameResults);
    }

    [TestMethod]
    public async Task GetGameResultById_ShouldReturnOKObjectWith200_WhenGameResultExist()
    {
        //Arrange
        int validId = 1;
        GameResult gameResult = new GameResult
        {
            Id = 1,
            Gameday = 1,
            TeamOne = "Team One",
            TeamTwo = "Team Two",
            TeamOneMatchPoints = 1,
            TeamTwoMatchPoints = 4
        };

        this._gameResultService.GetById(validId).Returns(gameResult);

        // Act
        ObjectResult? result = await this._controller.GetGameResultsById(validId) as ObjectResult;

        // Assert
        result.Value.Should().BeEquivalentTo(gameResult);
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}