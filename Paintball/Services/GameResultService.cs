using Microsoft.IdentityModel.Tokens;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Services;
using Paintball.Database.Abstractions.Entities;
using Paintball.Database.Abstractions.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Paintball.Services
{
    public class GameResultService : IGameResultService

    {
        private IGameResultRepository GameResultRepository { get; }

        public GameResultService(IGameResultRepository gameResultRepository)
        {
            GameResultRepository = gameResultRepository;
        }

        public IList<GameResult> GetAll()
        {
            var gameResults = GameResultRepository.GetAllGameResults();
            if (gameResults.IsNullOrEmpty())
            {
                throw new GameResultsNotImportedException(ExceptionMessages.GameResultsNotImported);
            }

            return gameResults;
        }

        public GameResult GetById(int id)
        {
            var gameResult = GameResultRepository.GetGameResultById(id);
            if (gameResult == null)
            {
                throw new GameResultNotFoundException(ExceptionMessages.GameResultNotFound);
            }


            return gameResult;
        }

        public IList<GameResult> GetByName(string teamName)
        {
            var gameResults = GameResultRepository.GetAllGameResultsByTeamName(teamName);
            if (gameResults.IsNullOrEmpty())
            {
                throw new GameResultNotFoundException(ExceptionMessages.GameResultNotFound);
            }

            return gameResults;
        }

        [ExcludeFromCodeCoverage]
        public void Delete() => GameResultRepository.RemoveAllGameResults();
    }
}