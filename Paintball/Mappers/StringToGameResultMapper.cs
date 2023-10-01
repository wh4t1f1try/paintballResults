using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Mappers;
using Paintball.Database.Abstractions.Entities;


namespace Paintball.Mappers
{
    public class StringToGameResultMapper : IStringToGameResultMapper

    {
        public IList<GameResult> MapGameResult(IList<string[]> dataRecords)
        {
            IList<GameResult> gameResults = new List<GameResult>();

            foreach (var dataString in dataRecords)
            {
                gameResults.Add(MapGameResult(dataString));
            }

            return gameResults;
        }

        public GameResult MapGameResult(string[] dataRecord)
        {
            GameResult gameResult = new()
            {
                Id = ParseStringToExpectedValue(dataRecord[0]),
                Gameday = ParseStringToExpectedValue(dataRecord[1]),
                TeamOne = dataRecord[2],
                TeamTwo = dataRecord[3],
                TeamOneMatchPoints = ParseStringToExpectedValue(dataRecord[4]),
                TeamTwoMatchPoints = ParseStringToExpectedValue(dataRecord[5])
            };
        
            return gameResult;
        }

        private int ParseStringToExpectedValue(string item)
        {
            if (!int.TryParse(item, out var number))
            {
                throw new NotAbleToParseException(ExceptionMessages.ParseError);
            }

            return number;
        }
    }
}