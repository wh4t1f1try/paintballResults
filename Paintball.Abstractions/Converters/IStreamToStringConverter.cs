namespace Paintball.Abstractions.Converters
{
    public interface IStreamToStringConverter
    {
        IList<string> Convert(Stream? stream);
    }
}