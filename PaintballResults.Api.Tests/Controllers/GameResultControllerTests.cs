namespace PaintballResults.Api.Tests.Controllers;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Paintball.Abstractions.DTOs;
using Paintball.Abstractions.Services;
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
        IList<GameResultDto> gameResults = new List<GameResultDto>
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
        IList<GameResultDto> gameResults = new List<GameResultDto>();
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
        GameResultDto gameResult = new GameResultDto
        {
            Id = 1,
            GameDay = 1,
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

    [TestMethod]
    public async Task GetAllResultsFromTeam_ShouldReturnOkResult_WithCorrectGameResults()
    {
        // Arrange
        string team = "TeamOne";
        List<GameResultDto> gameResults = new List<GameResultDto>
        {
            new GameResultDto
            {
                Id = 1, GameDay = 1, TeamOne = team, TeamTwo = "TeamTwo1", TeamOneMatchPoints = 10,
                TeamTwoMatchPoints = 20
            },
            new GameResultDto
            {
                Id = 2, GameDay = 2, TeamOne = team, TeamTwo = "TeamTwo2", TeamOneMatchPoints = 30,
                TeamTwoMatchPoints = 40
            }
        };
        this._gameResultService.GetByName(team).Returns(gameResults);
        // Act
        IActionResult result = await this._controller.GetAllResultsFromTeam(team);
        // Assert
        OkObjectResult? okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        IList<GameResultDto>? returnedGameResults =
            okResult.Value.Should().BeAssignableTo<IList<GameResultDto>>().Subject;
        returnedGameResults.Should().BeEquivalentTo(gameResults);
    }
}