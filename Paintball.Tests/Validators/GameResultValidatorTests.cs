using FluentAssertions;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Database.Abstractions.Entities;
using Paintball.Validators;

namespace Paintball.Tests.Validators;

[TestClass]
public class GameResultValidatorTests
{
    private GameResultValidator Validator { get; set; }


    [TestInitialize]
    public void Setup()
    {
        Validator = new GameResultValidator();
    }


    [TestMethod]
    public void GetValidGameResult_NotThrow_When_GameResultIsValid()
    {
        //Arrange
        var gameResult = CreateValidGameResult();

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(-3)]
    [DataRow(-168347)]
    public void GetValidGameResult_OrThrow_InvalidGameResultException_When_IdNotValid(int id)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.Id = id;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().Throw<InvalidGameResultException>()
            .WithMessage(ExceptionMessages.IdIsNotValid);
    }


    [TestMethod]
    [DataRow(1)]
    [DataRow(int.MaxValue)]
    public void GetValidGameResult_NotThrowException_When_IdIsValid(int id)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.Id = id;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(5)]
    [DataRow(579)]
    [DataRow(-579)]
    public void GetValidGameResult_OrThrow_InvalidGameResultException_When_GameDayNotValid(int gameDay)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.Gameday = gameDay;

        //Action
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().Throw<InvalidGameResultException>()
            .WithMessage(ExceptionMessages.GameDayIsNotValid);
    }


    [TestMethod]
    [DataRow(1)]
    [DataRow(4)]
    public void GetValidGameResult_NotThrow_WhenGameDayIsValid(int gameDay)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.Gameday = gameDay;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }


    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void GetValidGameResult_OrThrow_InvalidGameResultException_When_TeamOneNotValid(string? teamOne)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOne = teamOne;

        //Action
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().Throw<InvalidGameResultException>()
            .WithMessage(ExceptionMessages.TeamNotValid);
    }


    [TestMethod]
    [DataRow("Lucky Bastards")]
    [DataRow("Nordic Skullz")]
    public void GetValidGameResult_NotThrow_When_TeamOneIsValid(string? teamOne)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOne = teamOne;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void GetValidGameResult_OrThrow_InvalidGameResultException_When_TeamTwoNotValid(string? teamTwo)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOne = teamTwo;

        //Action
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().Throw<InvalidGameResultException>()
            .WithMessage(ExceptionMessages.TeamNotValid);
    }


    [TestMethod]
    [DataRow("Lucky Bastards")]
    [DataRow("Nordic Skullz")]
    public void GetValidGameResult_NotThrow_When_TeamTwoIsValid(string? teamTwo)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOne = teamTwo;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(11)]
    public void GetValidGameResult_OrThrow_InvalidGameResultException_When_TeamOneMatchPointsNotValid(
        int teamOneMatchPoints)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOneMatchPoints = teamOneMatchPoints;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().Throw<InvalidGameResultException>()
            .WithMessage(ExceptionMessages.MatchPointsNotValid);
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(10)]
    public void GetValidGameResult_NotThrow_When_TeamOneMatchPointsValid(int teamOneMatchPoints)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOneMatchPoints = teamOneMatchPoints;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(11)]
    public void GetValidGameResult_OrThrow_InvalidGameResultException_When_TeamTwoMatchPointsNotValid(
        int teamTwoMatchPoints)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOneMatchPoints = teamTwoMatchPoints;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().Throw<InvalidGameResultException>()
            .WithMessage(ExceptionMessages.MatchPointsNotValid);
    }


    [TestMethod]
    [DataRow(0)]
    [DataRow(10)]
    public void GetValidGameResult_NotThrow_When_TeamTwoMatchPointsValid(int teamTwoMatchPoints)
    {
        //Arrange
        var gameResult = CreateValidGameResult();
        gameResult.TeamOneMatchPoints = teamTwoMatchPoints;

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        action.Should().NotThrow();
    }


    [TestMethod]
    public void Validate_When_CalledWithListOfGameResults_NotThrowException()
    {
        //Arrange
        IList<GameResult> gameResults = new List<GameResult>
        {
            CreateValidGameResult()
        };

        //Act
        Action action = delegate { Validator.Validate(gameResults); };

        //Assert
        action.Should().NotThrow();
    }


    private GameResult CreateValidGameResult()
    {
        GameResult result = new()
        {
            Id = 1,
            Gameday = 1,
            TeamOne = "Lucky Bastards",
            TeamTwo = "Nordic Skullz",
            TeamOneMatchPoints = 0,
            TeamTwoMatchPoints = 4
        };

        return result;
    }


    [TestMethod]
    public void CreateValidGameResultReturnValidGameResult()
    {
        //Arrange
        var gameResult = CreateValidGameResult();

        //Act
        var action = () => { Validator.Validate(gameResult); };

        //Assert
        gameResult.Id.Should().Be(1);
        gameResult.Gameday.Should().Be(1);
        gameResult.TeamOne.Should().Be("Lucky Bastards");
        gameResult.TeamTwo.Should().Be("Nordic Skullz");
        gameResult.TeamOneMatchPoints.Should().Be(0);
        gameResult.TeamTwoMatchPoints.Should().Be(4);
        action.Should().NotThrow();
        action.Should().NotBeNull();
    }
}