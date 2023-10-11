namespace Paintball.Abstractions.Mappers;

using Paintball.Abstractions.DTOs;
using Paintball.Database.Abstractions.Entities;

public interface IGameResultMapper
{
    GameResultDto Map(GameResult gameResult);

    IList<GameResultDto> Map(IList<GameResult> gameResults);
}