namespace Paintball.Abstractions.Validators
{
    public interface IDataRecordValidator
    {
        void Validate(IList<string[]> dataRecords);
    }
}