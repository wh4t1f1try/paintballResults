using Paintball.Database.Abstractions.Entities;

namespace Paintball.Abstractions.Validators
{
    public interface IDuplicatesChecker
    {
        void CheckDuplicates(IList<GameResult> gameResults);
    }
}