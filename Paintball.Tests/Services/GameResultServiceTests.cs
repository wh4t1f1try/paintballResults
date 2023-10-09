namespace Paintball.Tests.Services
{
    using FluentAssertions;
    using NSubstitute;
    using Paintball.Abstractions.Constants;
    using Paintball.Abstractions.Exceptions;
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Abstractions.Repositories;
    using Paintball.Services;

    [TestClass]
    public class GameResultServiceTests
    {
        [TestInitialize]
        public void Setup()
        {
            this.GameResultRepository = Substitute.For<IGameResultRepository>();
            this.GameResultService = new GameResultService(this.GameResultRepository);
        }

        [TestMethod]
        public void GetAll_TwoItemsInRepo_ReturnsCollectionOfGameResults_NotThrowsException()
        {
            IList<GameResult> gameResults = new List<GameResult>
            {
                new(),
                new()
            };

            this.GameResultRepository.GetAllGameResults().Returns(gameResults);

            IList<GameResult> result = this.GameResultService.GetAll();

            result.Count().Should().Be(2);
            this.GameResultService.Invoking(service => service.GetAll())
                .Should().NotThrow();
        }

        [TestMethod]
        public void GetAll_RepoIsEmpty_ThrowsGameResultNotImprotedException()
        {
            IList<GameResult> gameResults = new List<GameResult>();

            this.GameResultRepository.GetAllGameResults().Returns(gameResults);

            this.GameResultService.Invoking(service => service.GetAll())
                .Should().Throw<GameResultsNotImportedException>();
        }

        [TestMethod]
        public void GetById_WhenGameResultExists_ReturnsGameResult()
        {
            int gameId = 1;
            GameResult expectedGameResult = new GameResult { Id = gameId };
            this.GameResultRepository.GetGameResultById(gameId).Returns(expectedGameResult);

            GameResult result = this.GameResultService.GetById(gameId);

            result.Should().NotBeNull();
            result.Should().Be(expectedGameResult);
        }

        [TestMethod]
        public void GetById_WhenGameResultDoesNotExist_ThrowsGameResultNotFoundException()
        {
            int gameId = 2;
            this.GameResultRepository.GetGameResultById(gameId).Returns(null as GameResult);

            this.GameResultService.Invoking(service => service.GetById(gameId))
                .Should().Throw<GameResultNotFoundException>()
                .WithMessage(ExceptionMessages.GameResultNotFound);
        }

        [TestMethod]
        public void GetByName_WhenGameResultExist_ReturnsGameResult()
        {
            string teamOne = "Lucky Bastards";

            GameResult expectedGameResult = new GameResult { TeamOne = teamOne };
            IList<GameResult> gameResults = new List<GameResult>
            {
                expectedGameResult
            };

            this.GameResultRepository.GetAllGameResultsByTeamName(teamOne).Returns(gameResults);

            IList<GameResult> result = this.GameResultService.GetByName(teamOne);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IList<GameResult>>();
            result.Should().ContainSingle().Which.Should().Be(expectedGameResult);
        }

        [TestMethod]
        public void GetByName_WhenGameResultNotExist_ThrowsGameResultNotFoundException()
        {
            string teamName = "Team Not Exist";

            this.GameResultRepository.GetAllGameResultsByTeamName(teamName).Returns(new List<GameResult>());

            this.GameResultService.Invoking(service => service.GetByName(teamName))
                .Should().Throw<GameResultNotFoundException>()
                .WithMessage(ExceptionMessages.GameResultNotFound);
        }

        private GameResultService GameResultService { get; set; } = null!;
        private IGameResultRepository GameResultRepository { get; set; } = null!;
    }
}