namespace Paintball.Validators;

using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Validators;

public class CsvDataStringValidator : ICsvDataStringValidator

{
    private const int ValidDataStringLength = 11;

    private static readonly char[] delimiters = { ',', ';' };

    public void Validate(IList<string> dataStrings)
    {
        foreach (string? dataString in dataStrings)
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


    private static bool ContainsDelimiters(string input)
    {
        return input.Any(c => delimiters.Contains(c));
    }
}