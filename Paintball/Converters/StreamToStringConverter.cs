using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Converters;
using Paintball.Abstractions.Exceptions;
using System.Text;

namespace Paintball.Converters;

public class StreamToStringConverter : IStreamToStringConverter
{
    public IList<string> Convert(Stream? stream)
    {
        IList<string> dataRecords = new List<string>();

        if (IsInvalid(stream)) throw new StreamIsNullOrEmptyException(ExceptionMessages.StreamIsNullOrEmpty);

        //Encoding.UTF7 deprecated -> need for diacritics
#pragma warning disable SYSLIB0001
        using StreamReader streamReader = new(stream!, Encoding.UTF7);
#pragma warning restore SYSLIB0001

        while (!streamReader.EndOfStream) dataRecords.Add(streamReader.ReadLine()!);

        return dataRecords;
    }

    private bool IsInvalid(Stream? stream)
    {
        return stream == null || stream.Length <= 0;
    }
}