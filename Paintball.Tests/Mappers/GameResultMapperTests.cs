namespace Paintball.Tests.Mappers;

using FluentAssertions;
using Paintball.Abstractions.DTOs;
using Paintball.Database.Abstractions.Entities;
using Paintball.Mappers;

[TestClass]
public class GameResultMapperTests
{
    [TestInitialize]
    public void Setup()
    {
        this.GameResultMapper = new GameResultMapper();
    }

    [TestMethod]
    public void MapToGameResultDto_Returns_GameResultDtoWithGivenValues()
    {
        //Arrange

        GameResult gameResult = new()
        {
            Id = 1,
            GameDay = 1,
            TeamOne = "Team One",
            TeamTwo = "Team Two",
            TeamOneMatchPoints = 2,
            TeamTwoMatchPoints = 1,
        };
        //Act
        GameResultDto result = this.GameResultMapper!.Map(gameResult);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.GameDay.Should().Be(1);
        result.TeamOne.Should().Be("Team One");
        result.TeamTwo.Should().Be("Team Two");
        result.TeamOneMatchPoints.Should().Be(2);
        result.TeamTwoMatchPoints.Should().Be(1);
    }

    [TestMethod]
    public void Map_ShouldReturnCorrectDtoList_WhenGameResultsListIsNotNull()
    {
        // Arrange
        List<GameResult> gameResults = new()
        {
            new GameResult
            {
                Id = 1, GameDay = 1, TeamOne = "Team One", TeamTwo = "Team Two", TeamOneMatchPoints = 1,
                TeamTwoMatchPoints = 1
            },
            new GameResult
            {
                Id = 2, GameDay = 2, TeamOne = "Team One", TeamTwo = "Team Two", TeamOneMatchPoints = 2,
                TeamTwoMatchPoints = 2
            }
        };

        // Act
        IList<GameResultDto> gameResultDtos = this.GameResultMapper!.Map(gameResults);

        // Assert
        gameResultDtos.Should().NotBeNull();
        gameResultDtos.Should().HaveCount(gameResults.Count);
        gameResults[0].Should().BeEquivalentTo(gameResultDtos[0]);
        gameResults[1].Should().BeEquivalentTo(gameResultDtos[1]);
    }

    private GameResultMapper? GameResultMapper { get; set; }
}