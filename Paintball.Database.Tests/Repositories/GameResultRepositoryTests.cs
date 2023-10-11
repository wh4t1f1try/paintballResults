namespace Paintball.Database.Tests.Repositories
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Contexts;
    using Paintball.Database.Repositories;

    [TestClass]
    public class GameResultRepositoryTests
    {
        private readonly GameResultContext _context;

        private readonly IList<GameResult> _gameResults;
        private readonly GameResultRepository _repository;

        public GameResultRepositoryTests()
        {
            DbContextOptions<GameResultContext> dbContextOptions =
                new DbContextOptionsBuilder<GameResultContext>().UseInMemoryDatabase("dbname").Options;
            this._context = new GameResultContext(dbContextOptions);

            this._repository = new GameResultRepository(this._context);

            this._gameResults = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    GameDay = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Lucky Bastards",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 6
                },

                new()
                {
                    Id = 2,
                    GameDay = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Breakout Spa",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 5
                },

                new()
                {
                    Id = 3,
                    GameDay = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Cologne Predators",
                    TeamOneMatchPoints = 0,
                    TeamTwoMatchPoints = 4
                }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            this._context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllGameResults_Should_ReturnAllGameResults_WhenInvoked()
        {
            //Arrange
            this._context.GameResults.AddRange(this._gameResults);
            this._context.SaveChanges();

            //Act
            IList<GameResult> results = this._repository.GetAllGameResults();


            //Assert
            results.Should().BeEquivalentTo(this._gameResults);
            results.Should().HaveCount(this._gameResults.Count);
            results.First().Id.Should().Be(1);
            results.Last().Id.Should().Be(3);
        }

        [TestMethod]
        //[DataRow(2)]
        public void GetGameResultById_Should_ReturnGameResultById()
        {
            //Arrange
            int id = 1;
            this._context.GameResults.AddRange(this._gameResults);
            this._context.SaveChanges();

            //Act
            GameResult result = this._repository.GetGameResultById(id);

            //Assert
            result.Id.Should().Be(id);
        }

        [TestMethod]
        //[DataRow("Braindead Emsdetten")]
        public void GetAllGameResultsByTeamName_Should_ReturnGameResultByTeamName_WhenInvoked()
        {
            //Arrange
            string teamName = "Braindead Emsdetten";
            this._context.GameResults.AddRange(this._gameResults);
            this._context.SaveChanges();

            //Act
            IList<GameResult> result = this._repository.GetAllGameResultsByTeamName(teamName);

            //Assert
            result.Should().HaveCount(3);
            result.First().TeamOne.Should().Be(teamName);
            result.Last().TeamOne.Should().Be(teamName);
        }

        [TestMethod]
        public void InsertAllGameResults_Should_InsertAllGameResults_WhenListIsEmpty()
        {
            //Arrange
            IList<GameResult> gameResults = new List<GameResult>();
            this._context.GameResults.AddRange(gameResults);
            this._context.SaveChanges();

            //Act
            Action action = () => this._repository.InsertAllGameResults(gameResults);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void InsertAllGameResults_ShouldUpdateResults_WhenDatabaseContainsExistingResults()
        {
            // Arrange
            List<GameResult> initialData = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    GameDay = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Lucky Bastards",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 3
                }
            };
            this._context.GameResults.AddRange(initialData);
            this._context.SaveChanges();

            // Arrange
            List<GameResult> updatedData = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    GameDay = 2,
                    TeamOne = "Wanderers Bremen",
                    TeamTwo = "Cologne Predators",
                    TeamOneMatchPoints = 3,
                    TeamTwoMatchPoints = 2
                }
            };

            // Act
            this._repository.InsertAllGameResults(updatedData);

            // Assert
            GameResult result = this._context.GameResults.FirstOrDefault(r => r.Id == 1)!;

            result.GameDay.Should().Be(2);
            result.TeamOne.Should().Be("Wanderers Bremen");
            result.TeamTwo.Should().Be("Cologne Predators");
            result.TeamOneMatchPoints.Should().Be(3);
            result.TeamTwoMatchPoints.Should().Be(2);
        }

        [TestMethod]
        public void InsertAllGameResults_ShouldClearDatabase_WhenInputListIsEmpty()
        {
            // Arrange: Insert initial data
            List<GameResult> initialData = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    GameDay = 1,
                    TeamOne = "Team A",
                    TeamTwo = "Team B",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 3
                }
            };
            this._context.GameResults.AddRange(initialData);
            this._context.SaveChanges();

            // Act: Insert empty list
            this._repository.InsertAllGameResults(new List<GameResult>());

            // Assert
            IList<GameResult> results = this._repository.GetAllGameResults();
            Assert.AreEqual(0, results.Count);
        }
    }
}