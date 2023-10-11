namespace Paintball.Mappers
{
    using Paintball.Abstractions.Constants;
    using Paintball.Abstractions.Exceptions;
    using Paintball.Abstractions.Mappers;
    using Paintball.Database.Abstractions.Entities;

    public class StringToGameResultMapper : IStringToGameResultMapper

    {
        public IList<GameResult> MapGameResult(IList<string[]> dataRecords)
        {
            IList<GameResult> gameResults = new List<GameResult>();

            foreach (string[] dataString in dataRecords)
            {
                gameResults.Add(this.MapGameResult(dataString));
            }

            return gameResults;
        }

        public GameResult MapGameResult(string[] dataRecord)
        {
            GameResult gameResult = new()
            {
                Id = ParseStringToExpectedValue(dataRecord[0]),
                GameDay = ParseStringToExpectedValue(dataRecord[1]),
                TeamOne = dataRecord[2],
                TeamTwo = dataRecord[3],
                TeamOneMatchPoints = ParseStringToExpectedValue(dataRecord[4]),
                TeamTwoMatchPoints = ParseStringToExpectedValue(dataRecord[5])
            };

            return gameResult;
        }

        private static int ParseStringToExpectedValue(string item)
        {
            if (!int.TryParse(item, out int number))
            {
                throw new NotAbleToParseException(ExceptionMessages.ParseError);
            }

            return number;
        }
    }
}