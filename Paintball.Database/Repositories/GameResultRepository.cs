using Paintball.Database.Abstractions.Entities;
using Paintball.Database.Abstractions.Repositories;
using Paintball.Database.Contexts;


namespace Paintball.Database.Repositories
{
    public class GameResultRepository : IGameResultRepository
    {
        private readonly GameResultContext _gameResultContext;

        public GameResultRepository(GameResultContext gameResultContext)
        {
            _gameResultContext = gameResultContext;
        }

        public GameResult GetGameResultById(int id) => _gameResultContext.Gameresults.Find(id)!;

        public IList<GameResult> GetAllGameResults() => _gameResultContext.Gameresults.ToList();

        public IList<GameResult> GetAllGameResultsByTeamName(string teamName)
        {
            var gameResultsFromTeam = _gameResultContext.Gameresults
                .Where(result => result.TeamOne.Contains(teamName) || result.TeamTwo.Contains(teamName))
                .ToList();
            return gameResultsFromTeam;
        }

        public void InsertAllGameResults(IList<GameResult> gameResults)
        {
            _gameResultContext.RemoveRange(_gameResultContext.Gameresults);
            _gameResultContext.Gameresults.AddRange(gameResults);
            Save();
        }

        public void RemoveAllGameResults()
        {
            var entitiesToRemove = _gameResultContext.Gameresults.ToList();
            _gameResultContext.Gameresults.RemoveRange(entitiesToRemove);
            Save();
        }

        public void Save() => _gameResultContext.SaveChanges();
    }
}