namespace Paintball.Abstractions.Validators
{
    using Paintball.Database.Abstractions.Entities;

    public interface IGameResultValidator
    {
        void Validate(GameResult result);
        public void Validate(IList<GameResult> gameResults);
    }
}