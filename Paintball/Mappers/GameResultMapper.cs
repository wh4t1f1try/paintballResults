namespace Paintball.Mappers;

using Paintball.Abstractions.DTOs;
using Paintball.Abstractions.Mappers;
using Paintball.Database.Abstractions.Entities;

public class GameResultMapper : IGameResultMapper
{
    /// <inheritdoc />
    public GameResultDto Map(GameResult? gameResult)
    {
        GameResultDto gameResultDto = new();

        if (gameResult != null)
        {
            {
                gameResultDto.Id = gameResult.Id;
                gameResultDto.GameDay = gameResult.GameDay;
                gameResultDto.TeamOne = gameResult.TeamOne;
                gameResultDto.TeamTwo = gameResult.TeamTwo;
                gameResultDto.TeamOneMatchPoints = gameResult.TeamOneMatchPoints;
                gameResultDto.TeamTwoMatchPoints = gameResult.TeamTwoMatchPoints;
            }
        }

        return gameResultDto;
    }


    /// <inheritdoc />
    public IList<GameResultDto> Map(IList<GameResult>? gameResults)
    {
        IList<GameResultDto> gameResultDtos = new List<GameResultDto>();

        if (gameResults != null)
        {
            foreach (GameResult gameResult in gameResults)
            {
                GameResultDto gameResultDto = new()
                {
                    Id = gameResult.Id,
                    GameDay = gameResult.GameDay,
                    TeamOne = gameResult.TeamOne,
                    TeamTwo = gameResult.TeamTwo,
                    TeamOneMatchPoints = gameResult.TeamOneMatchPoints,
                    TeamTwoMatchPoints = gameResult.TeamTwoMatchPoints
                };

                gameResultDtos.Add(gameResultDto);
            }
        }

        return gameResultDtos;
    }
}