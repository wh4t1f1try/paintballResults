namespace PaintballResults.Api.Tests.Domain.Paintball.Tests.Mappers;

using FluentAssertions;
using global::Paintball.Abstractions.Constants;
using global::Paintball.Abstractions.Exceptions;
using global::Paintball.Database.Abstractions.Entities;
using global::Paintball.Mappers;

[TestClass]
public class StringToGameResultMapperTests
{
    [TestInitialize]
    public void Setup()
    {
        this.Mapper = new StringToGameResultMapper();
    }

    [TestMethod]
    public void MapGameresult_ValidData_ReturnGameResult_NotThrowsException()
    {
        // Arrange
        string[] validDataRecord =
        {
            "1", "1", "Wanderers Bremen", "Lucky Bastards", "1", "4"
        };

        // Act
        GameResult gameResult = this.Mapper.MapGameResult(validDataRecord);

        // Assert
        gameResult.Should().NotBeNull();
        gameResult.Id.Should().Be(1);
        gameResult.Gameday.Should().Be(1);
        gameResult.TeamOne.Should().Be("Wanderers Bremen");
        gameResult.TeamTwo.Should().Be("Lucky Bastards");
        gameResult.TeamOneMatchPoints.Should().Be(1);
        gameResult.TeamTwoMatchPoints.Should().Be(4);

        this.Mapper.Invoking(y => y.MapGameResult(validDataRecord))
            .Should().NotThrow();
    }

    [TestMethod]
    public void MapGameResult_ValidData_ReturnGameResultCollection_NotThrwosException()
    {
        IList<string[]> validDataRecords = new List<string[]>();

        string[] dataRecord =
        {
            "1", "1", "Wanderers Bremen", "Lucky Bastards", "1", "4"
        };

        validDataRecords.Add(dataRecord);

        IList<GameResult> gameResults = this.Mapper.MapGameResult(validDataRecords);

        gameResults.Should().NotBeEmpty()
            .And.HaveCount(1);

        gameResults[0].Id.Should().Be(1);
        gameResults[0].Id.Should().Be(1);
        gameResults[0].Gameday.Should().Be(1);
        gameResults[0].TeamOne.Should().Be("Wanderers Bremen");
        gameResults[0].TeamTwo.Should().Be("Lucky Bastards");
        gameResults[0].TeamOneMatchPoints.Should().Be(1);
        gameResults[0].TeamTwoMatchPoints.Should().Be(4);

        this.Mapper.Invoking(m => m.MapGameResult(validDataRecords))
            .Should().NotThrow();
    }

    [TestMethod]
    public void MapGameResult_InvalidData_ThrowsNotAbleToParseExceptionWithCorrectMessage()
    {
        string[] dataRecord =
        {
            "a", "1", "Wanderers Bremen", "Lucky Bastards", "0", "4"
        };


        this.Mapper.Invoking(m => m.MapGameResult(dataRecord))
            .Should().Throw<NotAbleToParseException>()
            .WithMessage(ExceptionMessages.ParseError);
    }

    public required StringToGameResultMapper Mapper { get; set; }
}