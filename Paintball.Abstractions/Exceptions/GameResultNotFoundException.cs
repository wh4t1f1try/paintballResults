namespace Paintball.Abstractions.Exceptions
{
    public class GameResultNotFoundException : Exception
    {
        public GameResultNotFoundException(string? message) : base(message)
        {
        }
    }
}