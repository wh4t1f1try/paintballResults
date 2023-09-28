namespace Paintball.Abstractions.Exceptions
{
    public class DuplicatedRecordsException : Exception
    {
        public DuplicatedRecordsException(string? message) : base(message)
        {
        }
    }
}