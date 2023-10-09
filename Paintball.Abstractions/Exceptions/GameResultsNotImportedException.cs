namespace Paintball.Abstractions.Exceptions
{
    public class GameResultsNotImportedException : Exception
    {
        public GameResultsNotImportedException(string? message)
            : base(message)
        {
        }
    }
}