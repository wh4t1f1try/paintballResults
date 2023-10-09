namespace Paintball.Abstractions.Validators
{
    using Paintball.Database.Abstractions.Entities;

    public interface IDuplicatesChecker
    {
        void CheckDuplicates(IList<GameResult> gameResults);
    }
}