namespace Paintball.Tests.Validators;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentAssertions;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Validators;

//lambda´s lassen die coverage nicht auf 100 steigen

[TestClass]
[ExcludeFromCodeCoverage]
public class CsvDataStringValidatorTests
{
    [TestInitialize]
    public void Setup()
    {
        this.Validator = new CsvDataStringValidator();
    }


    [TestMethod]
    public void Validate_When_ValidDataStrings_ThrowsNoException()
    {
        //Arrange
        IList<string> dataStrings = new List<string>
        {
            "1;1;Wanderers Bremen;Lucky Bastards;0;4",
            "1;1;Wanderers Bremen;Lucky Bastards;0;4"
        };

        //Act
        Action action = delegate { this.Validator!.Validate(dataStrings); };

        //Assert
        action.Should().NotThrow();
    }

    [TestMethod]
    public void Validate_When_DataStringHaveNoDelimiters_ThrowsInvalidDataStringExceptionWithCorrectMessage()
    {
        //Arrange
        IList<string> dataStrings = new List<string>
        {
            this.GenerateRandomString(12, 20)
        };

        //Act
        Action action = delegate { this.Validator!.Validate(dataStrings); };

        //Assert
        action.Should().Throw<InvalidDataStringException>().WithMessage(ExceptionMessages.NoDelimitersFound);
    }

    [TestMethod]
    public void Validate_When_DataStringIsNull_ThrowsInvalidDataStringExceptionWithCorrectMessage()
    {
        //Arrange
        IList<string> dataStrings = new List<string>
        {
            null!,
            null!
        };
        //Act
        Action action = delegate { this.Validator!.Validate(dataStrings); };

        //Assert
        action.Should().Throw<InvalidDataStringException>().WithMessage(ExceptionMessages.InvalidDataString);
    }

    [TestMethod]
    public void Validate_When_DataStringIsUnderExpectedLength_ThrowsInvalidDataExWithCorrectMessage()
    {
        //Arrange
        IList<string> dataStrings = new List<string>
        {
            this.GenerateRandomString(5, 10)
        };
        //Act
        Action action = delegate { this.Validator!.Validate(dataStrings); };

        //Assert
        action.Should().Throw<InvalidDataStringException>().WithMessage(ExceptionMessages.InvalidDataString);
    }

    [TestMethod]
    [DataRow("Spiel;Tag;TeamOne;TeamTwo;TeamOneMP;TeamTwoMP")]
    [DataRow("Spiel,Tag,TeamOne,TeamTwo,TeamOneMP,TeamTwoMP")]
    public void Validate_When_DataStringHaveExpectedDelimiters_ThrowsNoException(string dataString)
    {
        //Arrange
        IList<string> dataStrings = new List<string>
        {
            dataString
        };

        //Act
        Action action = delegate { this.Validator!.Validate(dataStrings); };

        //Assert
        action.Should().NotThrow();
    }

    [ExcludeFromCodeCoverage]
    private string GenerateRandomString(int start, int end)
    {
        if (start < 0 || end < 0 || start > end)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidArgument);
        }

        Random random = new Random();
        StringBuilder randomString = new();

        int randomLength = random.Next(start, end);

        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÜÖÄüöäß";


        for (int i = 0; i < randomLength; i++)
        {
            randomString.Append(letters[random.Next(letters.Length)]);
        }

        return randomString.ToString();
    }

    private CsvDataStringValidator? Validator { get; set; }
}