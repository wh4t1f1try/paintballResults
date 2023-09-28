namespace Paintball.Abstractions.Exceptions
{
    public class NotAbleToParseException : Exception
    {
        public NotAbleToParseException(string? message) : base(message)
        {
        }
    }
}