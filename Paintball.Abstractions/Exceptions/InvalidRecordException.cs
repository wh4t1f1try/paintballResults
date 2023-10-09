namespace Paintball.Abstractions.Exceptions
{
    public class InvalidRecordException : Exception
    {
        public InvalidRecordException(string? message)
            : base(message)
        {
        }
    }
}