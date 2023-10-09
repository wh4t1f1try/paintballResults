namespace Paintball.Abstractions.Mappers
{
    using Paintball.Database.Abstractions.Entities;

    public interface IStringToGameResultMapper
    {
        GameResult MapGameResult(string[] dataRecord);
        IList<GameResult> MapGameResult(IList<string[]> dataRecords);
    }
}