#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
    private GameResultController controller;
    private IGameResultService gameResultService;


    [TestInitialize]
    public void Setup()
    {
        this.gameResultService = Substitute.For<IGameResultService>();
        this.controller = new GameResultController(this.gameResultService);
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
        this.gameResultService.GetAll().Returns(gameResults);
        //Act
        IActionResult result = await this.controller.GetAllGameResults();
        //Assert
        OkObjectResult? okObjectResult = result as OkObjectResult;
        okObjectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okObjectResult.Value.Should().BeEquivalentTo(gameResults);
    }

    [TestMethod]
    public async Task GetAllGameResult_ShouldReturnOkObjectResultWith200_WhenNoGameResultsExist()
    {
        //Arrange
        IList<GameResultDto> gameResults = new List<GameResultDto>();
        this.gameResultService.GetAll().Returns(gameResults);
        //Act
        IActionResult result = await this.controller.GetAllGameResults();
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

        this.gameResultService.GetById(validId).Returns(gameResult);
        // Act
        ObjectResult? result = await this.controller.GetGameResultsById(validId) as ObjectResult;
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
        this.gameResultService.GetByName(team).Returns(gameResults);
        // Act
        IActionResult result = await this.controller.GetAllResultsFromTeam(team);
        // Assert
        OkObjectResult? okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        IList<GameResultDto>? returnedGameResults =
            okResult.Value.Should().BeAssignableTo<IList<GameResultDto>>().Subject;
        returnedGameResults.Should().BeEquivalentTo(gameResults);
    }
}