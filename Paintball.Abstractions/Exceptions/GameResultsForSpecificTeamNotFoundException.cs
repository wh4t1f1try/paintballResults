namespace Paintball.Abstractions.Exceptions
{
    public class GameResultsForSpecificTeamNotFoundException : Exception
    {
        public GameResultsForSpecificTeamNotFoundException(string message) : base(message)
        {
        }
    }
}