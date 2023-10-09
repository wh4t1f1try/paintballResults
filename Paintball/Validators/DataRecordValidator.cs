namespace Paintball.Validators;

using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Validators;

public class DataRecordValidator : IDataRecordValidator
{
    private readonly IList<string> expectedHeaderRecord = new List<string>
        { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" };


    public void Validate(IList<string[]> dataRecords)
    {
        Queue<string[]> stringQ = new(dataRecords);

        this.ValidateHeader(stringQ.Dequeue());

        foreach (string[] dataRecord in dataRecords)
        {
            this.ValidateRecord(dataRecord);
        }
    }


    private void ValidateHeader(string[] header)
    {
        if (!this.HeaderIsEqual(header))
        {
            throw new InvalidRecordException(ExceptionMessages.InvalidHeader);
        }
    }

    private void ValidateRecord(string[] record)
    {
        if (!this.RecordIsValid(record))
        {
            throw new InvalidRecordException(ExceptionMessages.InvalidRecord);
        }
    }


    private bool RecordIsValid(string[] record)
    {
        return record.Length == this.expectedHeaderRecord.Count && !record.Any(string.IsNullOrWhiteSpace);
    }

    private bool HeaderIsEqual(IList<string> header)
    {
        return header.SequenceEqual(this.expectedHeaderRecord);
    }
}