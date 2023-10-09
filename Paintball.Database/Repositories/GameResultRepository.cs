namespace Paintball.Database.Repositories
{
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Abstractions.Repositories;
    using Paintball.Database.Contexts;

    public class GameResultRepository : IGameResultRepository
    {
        private readonly GameResultContext _gameResultContext;

        public GameResultRepository(GameResultContext gameResultContext)
        {
            this._gameResultContext = gameResultContext;
        }

        public GameResult GetGameResultById(int id)
        {
            return this._gameResultContext.GameResults.Find(id)!;
        }

        public IList<GameResult> GetAllGameResults()
        {
            return this._gameResultContext.GameResults.ToList();
        }

        public IList<GameResult> GetAllGameResultsByTeamName(string teamName)
        {
            List<GameResult> gameResultsFromTeam = this._gameResultContext.GameResults
                .Where(result => result.TeamOne.Contains(teamName) || result.TeamTwo.Contains(teamName))
                .ToList();
            return gameResultsFromTeam;
        }

        public void InsertAllGameResults(IList<GameResult> gameResults)
        {
            this._gameResultContext.RemoveRange(this._gameResultContext.GameResults);
            this._gameResultContext.GameResults.AddRange(gameResults);
            this._gameResultContext.SaveChanges();
        }
    }
}