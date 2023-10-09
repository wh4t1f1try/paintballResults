namespace Paintball.Abstractions.Exceptions
{
    public class StreamIsNullOrEmptyException : Exception
    {
        public StreamIsNullOrEmptyException(string? message)
            : base(message)
        {
        }
    }
}