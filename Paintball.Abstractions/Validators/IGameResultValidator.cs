using Paintball.Database.Abstractions.Entities;

namespace Paintball.Abstractions.Validators
{
    public interface IGameResultValidator
    {
        void Validate(GameResult result);
        public void Validate(IList<GameResult> gameResults);
    }
}