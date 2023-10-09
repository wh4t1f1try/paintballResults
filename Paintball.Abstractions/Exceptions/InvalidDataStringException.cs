namespace Paintball.Abstractions.Exceptions
{
    public class InvalidDataStringException : Exception
    {
        public InvalidDataStringException(string? message)
            : base(message)
        {
        }
    }
}