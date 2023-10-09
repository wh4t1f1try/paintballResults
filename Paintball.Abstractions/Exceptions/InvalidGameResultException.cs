namespace Paintball.Abstractions.Exceptions
{
    public class InvalidGameResultException : Exception
    {
        public InvalidGameResultException(string? message)
            : base(message)
        {
        }
    }
}