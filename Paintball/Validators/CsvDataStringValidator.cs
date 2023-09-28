using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Validators;


namespace Paintball.Validators
{
    public class CsvDataStringValidator : ICsvDataStringValidator

    {
        private const int ValidDataStringLength = 11;

        private static readonly char[] Delimiters = { ',', ';' };

        public void Validate(IList<string> dataStrings)
        {
            foreach (var dataString in dataStrings)
            {
                if (dataString == null || dataString.Length < ValidDataStringLength)
                {
                    throw new InvalidDataStringException(ExceptionMessages.InvalidDataString);
                }

                if (!ContainsDelimiters(dataString))
                {
                    throw new InvalidDataStringException(ExceptionMessages.NoDelimitersFound);
                }
            }
        }

        private static bool ContainsDelimiters(string input) => input.Any(c => Delimiters.Contains(c));
    }
}