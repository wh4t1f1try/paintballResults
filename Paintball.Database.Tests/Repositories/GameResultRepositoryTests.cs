using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Paintball.Database.Abstractions.Entities;
using Paintball.Database.Contexts;
using Paintball.Database.Repositories;

namespace Paintball.Database.Tests.Repositories
{
    [TestClass]
    public class GameResultRepositoryTests
    {
        private readonly GameResultRepository _repository;

        private readonly GameResultContext _context;

        private readonly IList<GameResult> _gameResults;

        public GameResultRepositoryTests()
        {
            var dbContextOptions =
                new DbContextOptionsBuilder<GameResultContext>().UseInMemoryDatabase("dbname").Options;
            _context = new GameResultContext(dbContextOptions);

            _repository = new GameResultRepository(_context);

            _gameResults = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    Gameday = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Lucky Bastards",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 6
                },

                new()
                {
                    Id = 2,
                    Gameday = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Breakout Spa",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 5
                },

                new()
                {
                    Id = 3,
                    Gameday = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Cologne Predators",
                    TeamOneMatchPoints = 0,
                    TeamTwoMatchPoints = 4
                }
            };
        }

        [TestCleanup]
        public void Cleanup() => _context.Database.EnsureDeleted();

        [TestMethod]
        public void GetAllGameResults_Should_ReturnAllGameResults_WhenInvoked()
        {
            //Arrange
            _context.Gameresults.AddRange(_gameResults);
            _context.SaveChanges();

            //Act
            var results = _repository.GetAllGameResults();


            //Assert
            results.Should().BeEquivalentTo(_gameResults);
            results.Should().HaveCount(_gameResults.Count);
            results.First().Id.Should().Be(1);
            results.Last().Id.Should().Be(3);
        }

        [TestMethod]
        //[DataRow(2)]
        public void GetGameResultById_Should_ReturnGameResultById()
        {
            //Arrange
            var id = 1;
            _context.Gameresults.AddRange(_gameResults);
            _context.SaveChanges();

            //Act
            var result = _repository.GetGameResultById(id);

            //Assert
            result.Id.Should().Be(id);
        }

        [TestMethod]
        //[DataRow("Braindead Emsdetten")]
        public void GetAllGameResultsByTeamName_Should_ReturnGameResultByTeamName_WhenInvoked()
        {
            //Arrange
            var teamName = "Braindead Emsdetten";
            _context.Gameresults.AddRange(_gameResults);
            _context.SaveChanges();

            //Act
            var result = _repository.GetAllGameResultsByTeamName(teamName);

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
            _context.Gameresults.AddRange(gameResults);
            _context.SaveChanges();

            //Act
            var action = () => _repository.InsertAllGameResults(gameResults);

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void InsertAllGameResults_ShouldUpdateResults_WhenDatabaseContainsExistingResults()
        {
            // Arrange
            var initialData = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    Gameday = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Lucky Bastards",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 3
                }
            };
            _context.Gameresults.AddRange(initialData);
            _context.SaveChanges();

            // Arrange
            var updatedData = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    Gameday = 2,
                    TeamOne = "Wanderers Bremen",
                    TeamTwo = "Cologne Predators",
                    TeamOneMatchPoints = 3,
                    TeamTwoMatchPoints = 2
                }
            };

            // Act
            _repository.InsertAllGameResults(updatedData);

            // Assert
            var result = _context.Gameresults.FirstOrDefault(r => r.Id == 1)!;

            result.Gameday.Should().Be(2);
            result.TeamOne.Should().Be("Wanderers Bremen");
            result.TeamTwo.Should().Be("Cologne Predators");
            result.TeamOneMatchPoints.Should().Be(3);
            result.TeamTwoMatchPoints.Should().Be(2);
        }

        [TestMethod]
        public void InsertAllGameResults_ShouldClearDatabase_WhenInputListIsEmpty()
        {
            // Arrange: Insert initial data
            var initialData = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    Gameday = 1,
                    TeamOne = "Team A",
                    TeamTwo = "Team B",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 3
                }
            };
            _context.Gameresults.AddRange(initialData);
            _context.SaveChanges();

            // Act: Insert empty list
            _repository.InsertAllGameResults(new List<GameResult>());

            // Assert
            var results = _repository.GetAllGameResults();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void RemoveAllGameResults_ShouldDeleteAllGameResults_WhenInvoke()
        {
            //Arrange
            var gameResults = new List<GameResult>
            {
                new()
                {
                    Id = 1,
                    Gameday = 1,
                    TeamOne = "Braindead Emsdetten",
                    TeamTwo = "Lucky Bastards",
                    TeamOneMatchPoints = 2,
                    TeamTwoMatchPoints = 3
                }
            };
            _repository.InsertAllGameResults(gameResults);

            //Act
            _repository.RemoveAllGameResults();

            //Assert
            _repository.GetAllGameResults().Should().BeEmpty();
        }
    }
}