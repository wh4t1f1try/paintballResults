using FluentAssertions;
using NSubstitute;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Database.Abstractions.Entities;
using Paintball.Database.Abstractions.Repositories;
using Paintball.Services;

namespace Paintball.Tests.Services
{
    [TestClass]
    public class GameResultServiceTests
    {
        private GameResultService GameResultService { get; set; }
        private IGameResultRepository GameResultRepository { get; set; }

        [TestInitialize]
        public void Setup()
        {
            GameResultRepository = Substitute.For<IGameResultRepository>();
            GameResultService = new GameResultService(GameResultRepository);
        }

        [TestMethod]
        public void GetAll_TwoItemsInRepo_ReturnsCollectionOfGameResults_NotThrowsException()
        {
            IList<GameResult> gameResults = new List<GameResult>
            {
                new (),
                new ()
            };

            GameResultRepository.GetAllGameResults().Returns(gameResults);

            var result = GameResultService.GetAll();

            result.Count().Should().Be(2);
            GameResultService.Invoking(service => service.GetAll())
            .Should().NotThrow();
        }

        [TestMethod]
        public void GetAll_RepoIsEmpty_ThrowsGameResultNotImprotedException()
        {
            IList<GameResult> gameResults = new List<GameResult>();

            GameResultRepository.GetAllGameResults().Returns(gameResults);

            GameResultService.Invoking(service => service.GetAll())
                .Should().Throw<GameResultsNotImportedException>();
        }

        [TestMethod]
        public void GetById_WhenGameResultExists_ReturnsGameResult()
        {
            int gameId = 1;
            var expectedGameResult = new GameResult { Id = gameId };
            GameResultRepository.GetGameResultById(gameId).Returns(expectedGameResult);

            var result = GameResultService.GetById(gameId);

            result.Should().NotBeNull();
            result.Should().Be(expectedGameResult);
        }

        [TestMethod]
        public void GetById_WhenGameResultDoesNotExist_ThrowsGameResultNotFoundException()
        {
            int gameId = 2;
            GameResultRepository.GetGameResultById(gameId).Returns(null as GameResult);

            GameResultService.Invoking(service => service.GetById(gameId))
                .Should().Throw<GameResultNotFoundException>()
                .WithMessage(ExceptionMessages.GameResultNotFound);
        }

        [TestMethod]
        public void GetByName_WhenGameResultExist_ReturnsGameResult()
        {
            var teamOne = "Lucky Bastards";

            var expectedGameResult = new GameResult { TeamOne = teamOne };
            IList<GameResult> gameResults = new List<GameResult>
            {
                expectedGameResult
            };

            GameResultRepository.GetAllGameResultsByTeamName(teamOne).Returns(gameResults);

            var result = GameResultService.GetByName(teamOne);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IList<GameResult>>();
            result.Should().ContainSingle().Which.Should().Be(expectedGameResult);
        }

        [TestMethod]
        public void GetByName_WhenGameResultNotExist_ThrowsGameResultNotFoundException()
        {
            var teamName = "Team Not Exist";

            GameResultRepository.GetAllGameResultsByTeamName(teamName).Returns(new List<GameResult>());

            GameResultService.Invoking(service => service.GetByName(teamName))
                .Should().Throw<GameResultNotFoundException>()
                .WithMessage(ExceptionMessages.GameResultNotFound);
        }
    }
}