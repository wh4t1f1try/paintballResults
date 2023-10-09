namespace Paintball.Validators;

using Microsoft.IdentityModel.Tokens;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Validators;
using Paintball.Database.Abstractions.Entities;

public sealed class GameResultValidator : IGameResultValidator
{
    private const int SeasonStart = 1;

    private const int SeasonEnd = 4;

    private const int MinMatchPoints = 0;

    private const int MaxMatchPoints = 10;

    public void Validate(GameResult result)
    {
        if (!this.IsValidId(result.Id))
        {
            throw new InvalidGameResultException(ExceptionMessages.IdIsNotValid);
        }

        if (!this.IsValidGameday(result.Gameday))
        {
            throw new InvalidGameResultException(ExceptionMessages.GameDayIsNotValid);
        }

        if (result.TeamOne.IsNullOrEmpty())
        {
            throw new InvalidGameResultException(ExceptionMessages.TeamNotValid);
        }

        if (result.TeamTwo.IsNullOrEmpty())
        {
            throw new InvalidGameResultException(ExceptionMessages.TeamNotValid);
        }

        if (!this.IsValidTeamMatchPoints(result.TeamOneMatchPoints))
        {
            throw new InvalidGameResultException(ExceptionMessages.MatchPointsNotValid);
        }

        if (!this.IsValidTeamMatchPoints(result.TeamTwoMatchPoints))
        {
            throw new InvalidGameResultException(ExceptionMessages.MatchPointsNotValid);
        }
    }

    public void Validate(IList<GameResult> gameResults)
    {
        foreach (GameResult gameResult in gameResults)
        {
            this.Validate(gameResult);
        }
    }

    private bool IsValidId(int id)
    {
        return id >= 1;
    }


    private bool IsValidGameday(int gameday)
    {
        return gameday >= SeasonStart && gameday <= SeasonEnd;
    }


    private bool IsValidTeamMatchPoints(int matchPoints)
    {
        return matchPoints >= MinMatchPoints && matchPoints <= MaxMatchPoints;
    }
}