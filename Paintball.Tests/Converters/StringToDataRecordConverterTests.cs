namespace Paintball.Tests.Converters
{
    using FluentAssertions;
    using Paintball.Converters;

    [TestClass]
    public class StringToDataRecordConverterTests
    {
        [TestInitialize]
        public void Setup()
        {
            this.Converter = new StringToDataRecordConverter();
        }

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
            IList<string[]> result = this.Converter.Convert(dataStrings);

            //Assert
            for (int i = 0; i < expectedValues.Length; i++)
            {
                result[0][i].Should().Be(expectedValues[i]);
            }
        }

        private static IEnumerable<string[]> CreateValidDataStrings()
        {
            yield return new[] { "1;1;Wanderers Bremen;Lucky Bastards;0;1" };
        }

        private StringToDataRecordConverter Converter { get; set; } = null!;
    }
}