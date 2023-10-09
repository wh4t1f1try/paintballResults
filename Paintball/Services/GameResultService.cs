namespace Paintball.Services
{
    using Microsoft.IdentityModel.Tokens;
    using Paintball.Abstractions.Constants;
    using Paintball.Abstractions.Exceptions;
    using Paintball.Abstractions.Services;
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Abstractions.Repositories;

    public class GameResultService : IGameResultService

    {
        public GameResultService(IGameResultRepository gameResultRepository)
        {
            this.GameResultRepository = gameResultRepository;
        }

        public IList<GameResult> GetAll()
        {
            IList<GameResult> gameResults = this.GameResultRepository.GetAllGameResults();
            if (gameResults.IsNullOrEmpty())
            {
                throw new GameResultsNotImportedException(ExceptionMessages.GameResultsNotImported);
            }

            return gameResults;
        }

        public GameResult GetById(int id)
        {
            GameResult? gameResult = this.GameResultRepository.GetGameResultById(id);
            if (gameResult == null)
            {
                throw new GameResultNotFoundException(ExceptionMessages.GameResultNotFound);
            }


            return gameResult;
        }

        public IList<GameResult> GetByName(string teamName)
        {
            IList<GameResult> gameResults = this.GameResultRepository.GetAllGameResultsByTeamName(teamName);
            if (gameResults.IsNullOrEmpty())
            {
                throw new GameResultNotFoundException(ExceptionMessages.GameResultNotFound);
            }

            return gameResults;
        }

        private IGameResultRepository GameResultRepository { get; }
    }
}