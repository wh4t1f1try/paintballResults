using Paintball.Database.Abstractions.Entities;

namespace Paintball.Database.Abstractions.Repositories
{
    public interface IGameResultRepository
    {
        GameResult GetGameResultById(int id);

        IList<GameResult> GetAllGameResults();

        IList<GameResult> GetAllGameResultsByTeamName(string teamName);

        void InsertAllGameResults(IList<GameResult> gameResults);

        void RemoveAllGameResults();

        void Save();
    }
}