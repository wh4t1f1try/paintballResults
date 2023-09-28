namespace Paintball.Abstractions.Converters
{
    public interface IStringToDataRecordConverter
    {
        IList<string[]> Convert(IList<string> dataStrings);
    }
}