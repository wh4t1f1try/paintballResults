using Paintball.Database.Abstractions.Entities;

namespace Paintball.Abstractions.Mappers
{
    public interface IStringToGameResultMapper
    {
        GameResult MapGameResult(string[] dataRecord);
        IList<GameResult> MapGameResult(IList<string[]> dataRecords);
    }
}