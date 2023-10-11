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
                Id = this.ParseStringToExpectedValue(dataRecord[0]),
                GameDay = this.ParseStringToExpectedValue(dataRecord[1]),
                TeamOne = dataRecord[2],
                TeamTwo = dataRecord[3],
                TeamOneMatchPoints = this.ParseStringToExpectedValue(dataRecord[4]),
                TeamTwoMatchPoints = this.ParseStringToExpectedValue(dataRecord[5])
            };

            return gameResult;
        }

        private int ParseStringToExpectedValue(string item)
        {
            if (!int.TryParse(item, out int number))
            {
                throw new NotAbleToParseException(ExceptionMessages.ParseError);
            }

            return number;
        }
    }
}