namespace Paintball.Tests.Validators;

using FluentAssertions;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Validators;

[TestClass]
public class DataRecordValidatorTests
{
    [TestInitialize]
    public void Setup()
    {
        this.DataRecordValidator = new DataRecordValidator();
    }


    [TestMethod]
    public void Validate_When_DataRecordsAreValid_ThrowsNoException()
    {
        //Arrange
        IList<string[]> validDataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "1", "4" }
        };
        //Act
        Action action = delegate { this.DataRecordValidator.Validate(validDataRecord); };
        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_When_HeaderIsNotValid_ThrowsInvalidRecordExceptionWithExpectedMessage()
    {
        //Arrange
        IList<string[]> invalidDataRecord = new List<string[]>
        {
            new[] { "Spiel", "", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "1", "4" }
        };
        //Act
        Action action = delegate { this.DataRecordValidator.Validate(invalidDataRecord); };
        //Assert 
        action.Should().Throw<InvalidRecordException>().WithMessage(ExceptionMessages.InvalidHeader);
    }

    [TestMethod]
    public void Validate_When_RecordDoesNotMatchExpectedRecordLength_ThrowsInvalidRecordExceptionWithExpectedMessage()
    {
        //Arrange
        IList<string[]> invalidDataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Lucky Bastards", "1", "4" }
        };
        //Act
        Action action = delegate { this.DataRecordValidator.Validate(invalidDataRecord); };
        //Assert
        action.Should().Throw<InvalidRecordException>().WithMessage(ExceptionMessages.InvalidRecord);
    }

    [TestMethod]
    public void Validate_When_RecordContainsWhiteSpace_ThrowsInvalidRecordExceptionWitchExpectedMessage()
    {
        //Arrange
        IList<string[]> invalidDataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Lucky Bastards", " ", "1", "4" }
        };
        //Act
        Action action = delegate { this.DataRecordValidator.Validate(invalidDataRecord); };
        //Assert
        action.Should().Throw<InvalidRecordException>().WithMessage(ExceptionMessages.InvalidRecord);
    }

    [TestMethod]
    public void Validate_When_RecordContainsEmptyString_ThrowsInvalidRecordExceptionWitchExpectedMessage()
    {
        //Arrange
        IList<string[]> invalidDataRecord = new List<string[]>
        {
            new[] { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" },
            new[] { "1", "1", "Lucky Bastards", string.Empty, "1", "4" }
        };
        //Act
        Action action = delegate { this.DataRecordValidator.Validate(invalidDataRecord); };
        //Assert
        action.Should().Throw<InvalidRecordException>().WithMessage(ExceptionMessages.InvalidRecord);
    }

    private DataRecordValidator DataRecordValidator { get; set; } = null!;
}