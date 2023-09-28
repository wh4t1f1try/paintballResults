using Paintball.Abstractions.Converters;

namespace Paintball.Converters
{
    public class StringToDataRecordConverter : IStringToDataRecordConverter
    {
        private static readonly char[] Delimiters = { ',', ';' };

        public IList<string[]> Convert(IList<string> dataStrings)
        {
            IList<string[]> dataRecords = new List<string[]>();

            foreach (var dataString in dataStrings)
            {
                dataRecords.Add(dataString.Split(Delimiters));
            }

            return dataRecords;
        }
    }
}