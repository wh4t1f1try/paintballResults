namespace Paintball.Services
{
    using Microsoft.IdentityModel.Tokens;
    using Paintball.Abstractions.Constants;
    using Paintball.Abstractions.DTOs;
    using Paintball.Abstractions.Exceptions;
    using Paintball.Abstractions.Mappers;
    using Paintball.Abstractions.Services;
    using Paintball.Database.Abstractions.Entities;
    using Paintball.Database.Abstractions.Repositories;

    public class GameResultService : IGameResultService

    {
        public GameResultService(IGameResultRepository gameResultRepository, IGameResultMapper gameResultMapper)
        {
            this.GameResultRepository = gameResultRepository;
            this.GameResultMapper = gameResultMapper;
        }

        public IList<GameResultDto> GetAll()
        {
            IList<GameResult> gameResults = this.GameResultRepository.GetAllGameResults();
            if (gameResults.IsNullOrEmpty())
            {
                throw new GameResultsNotImportedException(ExceptionMessages.GameResultsNotImported);
            }

            return this.GameResultMapper.Map(gameResults);
        }

        public GameResultDto GetById(int id)
        {
            GameResult? gameResult = this.GameResultRepository.GetGameResultById(id);
            if (gameResult == null)
            {
                throw new GameResultNotFoundException(ExceptionMessages.GameResultNotFound);
            }

            return this.GameResultMapper.Map(gameResult);
        }

        public IList<GameResultDto> GetByName(string teamName)
        {
            IList<GameResult> gameResults = this.GameResultRepository.GetAllGameResultsByTeamName(teamName);
            if (gameResults.IsNullOrEmpty())
            {
                throw new GameResultsForSpecificTeamNotFoundException(ExceptionMessages.GameResultForTeamNotFound);
            }

            return this.GameResultMapper.Map(gameResults);
        }

        private IGameResultMapper GameResultMapper { get; }
        private IGameResultRepository GameResultRepository { get; }
    }
}