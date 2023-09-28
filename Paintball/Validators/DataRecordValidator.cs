using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Validators;

namespace Paintball.Validators
{
    public class DataRecordValidator : IDataRecordValidator
    {
        private readonly IList<string> _headerRecord = new List<string>()
            { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" };

        public void Validate(IList<string[]> dataRecords)
        {
            Queue<string[]> stringQ = new Queue<string[]>(dataRecords);

            ValidateHeader(stringQ.Dequeue());

            foreach (var dataRecord in dataRecords)
            {
                ValidateRecord(dataRecord);
            }
        }

        private bool RecordIsValid(string[] record)
        {
            return record.Length == _headerRecord.Count && !record.Any(string.IsNullOrWhiteSpace);
        }

        private void ValidateHeader(string[] header)
        {
            if (!HeaderIsEqual(header))
                throw new InvalidRecordException(ExceptionMessages.InvalidHeader);
        }

        private void ValidateRecord(string[] record)
        {
            if (!RecordIsValid(record))
                throw new InvalidRecordException(ExceptionMessages.InvalidRecord);
        }

        private bool HeaderIsEqual(IList<string> header)
        {
            return header.SequenceEqual(_headerRecord);
        }
    }
}