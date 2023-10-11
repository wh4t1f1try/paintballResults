namespace Paintball.Tests.Services
{
    using FluentAssertions;
    using NSubstitute;
    using Paintball.Abstractions.Constants;
    using Paintball.Abstractions.DTOs;
    using Paintball.Abstractions.Exceptions;
    using Paintball.Abstractions.Mappers;
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
            this.GameResultMapper = Substitute.For<IGameResultMapper>();
            this.GameResultService = new GameResultService(this.GameResultRepository, this.GameResultMapper);
        }


        [TestMethod]
        public void GetAll_TwoItemsInRepo_ReturnsCollectionOfGameResults_NotThrowsException()
        {
            //Arrange
            IList<GameResult> gameResults = new List<GameResult>
            {
                new(),
                new()
            };

            IList<GameResultDto> gameResultDtos = new List<GameResultDto>
            {
                new(),
                new()
            };

            this.GameResultRepository.GetAllGameResults().Returns(gameResults);
            this.GameResultMapper.Map(gameResults).Returns(gameResultDtos);
            //Act
            IList<GameResultDto> result = this.GameResultService.GetAll();
            //Assert
            result.Count().Should().Be(2);
            this.GameResultService.Invoking(service => service.GetAll())
                .Should().NotThrow();
        }

        [TestMethod]
        public void GetAll_RepoIsEmpty_ThrowsGameResultNotImprotedException()
        {
            //Arrange
            IList<GameResult> gameResults = new List<GameResult>();
            //Act
            this.GameResultRepository.GetAllGameResults().Returns(gameResults);
            //Assert
            this.GameResultService.Invoking(service => service.GetAll())
                .Should().Throw<GameResultsNotImportedException>();
        }

        [TestMethod]
        public void GetById_WhenGameResultExists_ReturnsGameResultDto()
        {
            //Arrange
            int gameId = 1;
            GameResult expectedGameResult = new GameResult { Id = gameId };
            this.GameResultRepository.GetGameResultById(gameId).Returns(expectedGameResult);

            GameResultDto dto = new GameResultDto { Id = gameId };
            this.GameResultMapper.Map(expectedGameResult).Returns(dto);
            //Act
            GameResultDto result = this.GameResultService.GetById(gameId);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedGameResult);
        }

        [TestMethod]
        public void GetById_WhenGameResultDoesNotExist_ThrowsGameResultNotFoundException()
        {
            //Arrange
            int gameId = 2;
            this.GameResultRepository.GetGameResultById(gameId).Returns(null as GameResult);
            //Act - Assert
            this.GameResultService.Invoking(service => service.GetById(gameId))
                .Should().Throw<GameResultNotFoundException>()
                .WithMessage(ExceptionMessages.GameResultNotFound);
        }

        [TestMethod]
        public void GetByName_WhenGameResultExist_ReturnsGameResultDto()
        {
            //Arrange
            string teamOne = "Lucky Bastards";
            GameResult expectedGameResult = new GameResult { TeamOne = teamOne };
            IList<GameResult> expectedGameResults = new List<GameResult>
            {
                expectedGameResult
            };

            this.GameResultRepository.GetAllGameResultsByTeamName(teamOne).Returns(expectedGameResults);

            IList<GameResultDto> gameResultDtos = new List<GameResultDto>
            {
                new GameResultDto
                {
                    TeamOne = teamOne,
                }
            };

            this.GameResultMapper.Map(expectedGameResults).Returns(gameResultDtos);
            //Act
            IList<GameResultDto> result = this.GameResultService.GetByName(teamOne);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedGameResults);
        }

        [TestMethod]
        public void GetByName_WhenGameResultNotExist_ThrowsGameResultNotFoundException()
        {
            //Arrange
            string teamName = "Team Not Exist";
            this.GameResultRepository.GetAllGameResultsByTeamName(teamName).Returns(new List<GameResult>());
            //Act - Assert
            this.GameResultService.Invoking(service => service.GetByName(teamName))
                .Should().Throw<GameResultsForSpecificTeamNotFoundException>()
                .WithMessage(ExceptionMessages.GameResultForTeamNotFound);
        }

        private GameResultService GameResultService { get; set; } = null!;
        private IGameResultRepository GameResultRepository { get; set; } = null!;
        private IGameResultMapper GameResultMapper { get; set; } = null !;
    }
}