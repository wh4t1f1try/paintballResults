namespace Paintball.Database.Repositories
{
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Abstractions.Repositories;
    using Paintball.Database.Contexts;

    public class GameResultRepository : IGameResultRepository
    {
        private readonly GameResultContext gameResultContext;

        public GameResultRepository(GameResultContext gameResultContext)
        {
            this.gameResultContext = gameResultContext;
        }

        public GameResult GetGameResultById(int id)
        {
            return this.gameResultContext.GameResults.Find(id)!;
        }

        public IList<GameResult> GetAllGameResults()
        {
            return this.gameResultContext.GameResults.ToList();
        }

        public IList<GameResult> GetAllGameResultsByTeamName(string teamName)
        {
            List<GameResult> gameResultsFromTeam = this.gameResultContext.GameResults
                .Where(result => result.TeamOne.Contains(teamName) || result.TeamTwo.Contains(teamName))
                .ToList();
            return gameResultsFromTeam;
        }

        public void InsertAllGameResults(IList<GameResult> gameResults)
        {
            this.gameResultContext.RemoveRange(this.gameResultContext.GameResults);
            this.gameResultContext.GameResults.AddRange(gameResults);
            this.gameResultContext.SaveChanges();
        }
    }
}