namespace Paintball.Database.Abstractions.Repositories
{
    using Paintball.Database.Abstractions.Entities;

    public interface IGameResultRepository
    {
        GameResult GetGameResultById(int id);

        IList<GameResult> GetAllGameResults();

        IList<GameResult> GetAllGameResultsByTeamName(string teamName);

        void InsertAllGameResults(IList<GameResult> gameResults);
    }
}