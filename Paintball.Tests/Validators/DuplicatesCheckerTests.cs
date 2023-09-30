using FluentAssertions;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Database.Abstractions.Entities;
using Paintball.Validators;

namespace Paintball.Tests.Validators;

[TestClass]
public class DuplicatesCheckerTests
{
    private DuplicatesChecker _duplicatesChecker = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _duplicatesChecker = new DuplicatesChecker();
    }

    [TestMethod]
    public void CheckDuplicates_When_NoDuplicates_DoesNotThrowException()
    {
        // Arrange
        var gameResults = new List<GameResult>
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };
        // Act & Assert
        _duplicatesChecker.Invoking(y => y.CheckDuplicates(gameResults))
            .Should().NotThrow<DuplicatedRecordsException>();
    }

    [TestMethod]
    public void CheckDuplicates_When_OneItemInCollection_DoesNotThrowException()
    {
        // Arrange
        var gameResults = new List<GameResult>
        {
            new() { Id = 1 }
        };
        // Act & Assert
        _duplicatesChecker.Invoking(y => y.CheckDuplicates(gameResults))
            .Should().NotThrow<DuplicatedRecordsException>();
    }

    [TestMethod]
    public void CheckDuplicates_When_ContainsDuplicates_ThrowsException()
    {
        // Arrange
        var gameResults = new List<GameResult>
        {
            new() { Id = 1 },
            new() { Id = 1 },
            new() { Id = 2 }
        };
        // Act & Assert
        _duplicatesChecker.Invoking(y => y.CheckDuplicates(gameResults))
            .Should().Throw<DuplicatedRecordsException>()
            .WithMessage(ExceptionMessages.DuplicatedRecord);
    }

    [TestMethod]
    public void CheckDuplicates_When_EmptyList_DoesNotThrowException()
    {
        // Arrange
        var gameResults = new List<GameResult>();
        // Act & Assert
        _duplicatesChecker.Invoking(y => y.CheckDuplicates(gameResults))
            .Should().NotThrow<DuplicatedRecordsException>();
    }

    [TestMethod]
    public void CheckDuplicates_When_NullList_ThrowsException()
    {
        // Arrange
        IList<GameResult> gameResults = null!;
        // Act & Assert
        _duplicatesChecker.Invoking(y => y.CheckDuplicates(gameResults))
            .Should().Throw<ArgumentNullException>();
    }
}