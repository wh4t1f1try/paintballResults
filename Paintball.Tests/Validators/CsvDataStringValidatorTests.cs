using System.Text;
using FluentAssertions;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Validators;

namespace Paintball.Tests.Validators
{
    [TestClass]
    public class CsvDataStringValidatorTests
    {
        private CsvDataStringValidator? Validator { get; set; }


        [TestInitialize]
        public void Setup() => Validator = new CsvDataStringValidator();


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
            var action = () => { Validator.Validate(dataStrings); };

            //Assert
            action.Should().NotThrow();
        }

        [TestMethod]
        public void Validate_When_DataStringHaveNoDelimiters_ThrowsInvalidDataStringExceptionWithCorrectMessage()
        {
            //Arrange
            IList<string> dataStrings = new List<string>
            {
                GenerateRandomString(12, 20),
                GenerateRandomString(12, 70)
            };

            //Act
            var action = () => { Validator.Validate(dataStrings); };

            //Assert
            action.Should().Throw<InvalidDataStringException>().WithMessage(ExceptionMessages.NoDelimitersFound);
        }

        [TestMethod]
        public void Validate_When_DataStringIsNull_ThrowsInvalidDataStringExceptionWithCorrectMessage()
        {
            //Arrange
            IList<string> dataStrings = new List<string>
            {
                null,
                null
            };
            //Act
            var action = () => { Validator.Validate(dataStrings); };

            //Assert
            action.Should().Throw<InvalidDataStringException>().WithMessage(ExceptionMessages.InvalidDataString);
        }

        [TestMethod]
        public void Validate_When_DataStringIsUnderExpectedLength_ThrowsInvalidDataExWithCorrectMessage()
        {
            //Arrange
            IList<string> dataStrings = new List<string>
            {
                GenerateRandomString(5, 10)
            };
            //Act
            Action action = delegate { Validator.Validate(dataStrings); };

            //Assert
            action.Should().Throw<InvalidDataStringException>().WithMessage(ExceptionMessages.InvalidDataString);
        }


        private string GenerateRandomString(int start, int end)
        {
            if (start < 0 || end < 0 || start > end)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidArgument);
            }

            var random = new Random();
            StringBuilder randomString = new();

            var randomLength = random.Next(start, end);

            var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÜÖÄüöäß";


            for (var i = 0; i < randomLength; i++)
            {
                randomString.Append(letters[random.Next(letters.Length)]);
            }

            return randomString.ToString();
        }
    }
}