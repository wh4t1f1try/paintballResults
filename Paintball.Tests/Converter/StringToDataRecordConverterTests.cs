using FluentAssertions;
using Paintball.Converters;

namespace Paintball.Tests.Converters
{
    [TestClass]
    public class StringToDataRecordConverterTests
    {
        private StringToDataRecordConverter Converter { get; set; }


        [TestInitialize]
        public void Setup() => Converter = new StringToDataRecordConverter();

        [TestMethod]
        [DynamicData(nameof(CreateValidDataStrings), DynamicDataSourceType.Method)]
        public void Convert_When_Invoke_Return_ListOfDataStringsToDataArray(string dataString)
        {
            //Arrange
            IList<string> dataStrings = new List<string>
            {
                dataString
            };
            string[] expectedValues = { "1", "1", "Wanderers Bremen", "Lucky Bastards", "0", "1" };

            //Act
            var result = Converter.Convert(dataStrings);

            //Assert
            for (var i = 0; i < expectedValues.Length; i++)
            {
                result[0][i].Should().Be(expectedValues[i]);
            }
        }

        private static IEnumerable<string[]> CreateValidDataStrings()
        {
            yield return new[] { "1;1;Wanderers Bremen;Lucky Bastards;0;1" };
        }
    }
}