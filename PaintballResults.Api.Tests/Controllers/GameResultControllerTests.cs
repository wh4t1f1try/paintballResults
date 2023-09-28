using NSubstitute;
using Paintball.Abstractions.Services;
using Paintball.Database.Abstractions.Entities;
using PaintballResults.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace PaintballResults.Api.Tests.Controllers
{
    [TestClass]
    public class GameResultControllerTests
    {
        private IGameResultService _gameResultService;
        private GameResultController _controller;


        [TestInitialize]
        public void Setup()
        {
            _gameResultService = Substitute.For<IGameResultService>();
            _controller = new GameResultController(_gameResultService);
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
            _gameResultService.GetAll().Returns(gameResults);

            //Act
            var result = await _controller.GetAllGameResults();

            //Assert
            var okObjectResult = result as OkObjectResult;
            okObjectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
            okObjectResult.Value.Should().BeEquivalentTo(gameResults);
        }

        [TestMethod]
        public async Task GetAllGameResulst_ShouldReturnOkObjectResultWith200_WhenNoGameResultsExist()
        {
            //Arrange
            IList<GameResult> gameResults = new List<GameResult>();
            _gameResultService.GetAll().Returns(gameResults);

            //Act
            var result = await _controller.GetAllGameResults();

            //Assert
            var okObjectResult = result as OkObjectResult;
            okObjectResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
            okObjectResult.Value.Should().BeEquivalentTo(gameResults);
        }

        [TestMethod]
        public async Task GetGameResultById_ShouldReturnOKObjectWith200_WhenGameResultExist()
        {
            //Arrange
            var validId = 1;
            var gameResult = new GameResult
            {
                Id = 1,
                Gameday = 1,
                TeamOne = "Team One",
                TeamTwo = "Team Two",
                TeamOneMatchPoints = 1,
                TeamTwoMatchPoints = 4
            };

            _gameResultService.GetById(validId).Returns(gameResult);

            // Act
            var result = await _controller.GetGameResultsById(validId) as ObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(gameResult);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}