using System.Text;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Converters;
using Paintball.Abstractions.Exceptions;

namespace Paintball.Converters
{
    public class StreamToStringConverter : IStreamToStringConverter
    {
        public IList<string> Convert(Stream? stream)
        {
            IList<string> dataRecords = new List<string>();

            if (IsInvalid(stream))
            {
                throw new StreamIsNullOrEmptyException(ExceptionMessages.StreamIsNullOrEmpty);
            }

            using StreamReader streamReader = new(stream, Encoding.UTF7);

            while (!streamReader.EndOfStream)
            {
                dataRecords.Add(streamReader.ReadLine()!);
            }

            return dataRecords;
        }

        private bool IsInvalid(Stream? stream) => stream == null || stream.Length <= 0;
    }
}