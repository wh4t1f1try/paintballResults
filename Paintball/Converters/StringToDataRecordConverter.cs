namespace Paintball.Converters
{
    using Paintball.Abstractions.Converters;

    public class StringToDataRecordConverter : IStringToDataRecordConverter
    {
        private static readonly char[] delimiters = { ',', ';' };

        public IList<string[]> Convert(IList<string> dataStrings)
        {
            IList<string[]> dataRecords = new List<string[]>();

            foreach (string dataString in dataStrings)
            {
                dataRecords.Add(dataString.Split(delimiters));
            }

            return dataRecords;
        }
    }
}