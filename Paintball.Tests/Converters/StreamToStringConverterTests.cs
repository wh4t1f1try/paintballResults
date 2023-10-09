#pragma warning disable SYSLIB0001
namespace Paintball.Tests.Converters;

using System.Text;
using FluentAssertions;
using Paintball.Abstractions.Exceptions;
using Paintball.Converters;

[TestClass]
public class StreamToStringConverterTests
{
    [TestInitialize]
    public void Setup()
    {
        this.Converter = new StreamToStringConverter();
    }

    [TestMethod]
    [DynamicData(nameof(CreateTestRecords), DynamicDataSourceType.Method)]
    public void Convert_WhenStreamIsValid_Return_ListOfDataStrings(string content)
    {
        //Arrange

        MemoryStream stream = new(Encoding.UTF7.GetBytes(content));

        //Act
        IList<string> result = this.Converter.Convert(stream);

        //Assert
        result.Should().BeEquivalentTo(content);
        stream.Close();
    }

    [TestMethod]
    public void Convert_WhenStreamIsNull_Throws_Exception()
    {
        // Arrange
        Stream? stream = null;

        // Act
        Action action = () => this.Converter.Convert(stream);

        // Assert
        action.Should().Throw<StreamIsNullOrEmptyException>();
    }

    [TestMethod]
    public void Convert_WhenStreamIsEmpty_Throws_Exception()
    {
        //Arrange
        MemoryStream stream = new MemoryStream();

        //Act
        Action action = () => this.Converter.Convert(stream);

        //Assert
        action.Should().Throw<StreamIsNullOrEmptyException>();
        stream.Close();
    }

    private static IEnumerable<string[]> CreateTestRecords()
    {
        yield return new[] { "1; 1; Wanderers Bremen; Lucky Bastards; 0; 4 " };
        yield return new[] { "1; 2; Wanderers Bremen; Lucky Bastards; 0; 4 " };
        yield return new[] { "1; 1; Wanderers Bremen; Lucky Bastards; 3; 4 " };
        yield return new[] { "1; 1; Wanderers Bremen; Lucky Bastards; 0; 3 " };
    }

    private StreamToStringConverter Converter { get; set; } = null!;
}