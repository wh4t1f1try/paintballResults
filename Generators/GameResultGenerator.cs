using Paintball.Database.Abstractions.Entities;

namespace Generators;

public class GameResultGenerator
{
    private readonly Random _random = new();

    public GameResult GetGameResultWithRandomValues()
    {
        var gameResult = new GameResult();

        var gameResultType = gameResult.GetType();

        var properties = gameResultType.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(int))
            {
                if (property.Name == "Id")
                {
                    //value ShouldNotBeInRange 1 - MaxInt
                    var value = _random.Next(int.MinValue, 0);
                    property.SetValue(gameResult, value);
                }

                if (property.Name == "GameDay")
                {
                    //value ShouldNotBeInRange 1 - 4
                    int value;

                    do
                    {
                        value = _random.Next(int.MinValue, int.MaxValue);
                    } while (value < 5 && value > 0);

                    property.SetValue(gameResult, value);
                }

                if (property.Name.EndsWith("MatchPoints"))
                {
                    //value ShouldNotBeInRange  0 - 10
                    int value;

                    do
                    {
                        value = _random.Next(int.MinValue, int.MaxValue);
                    } while (value < 10 && value > -1);

                    property.SetValue(gameResult, value);
                }
            }

            if (property.PropertyType == typeof(string))
            {
                var randomString = Guid.NewGuid().ToString();
                property.SetValue(gameResult, randomString);
            }
        }

        return gameResult;
    }
}