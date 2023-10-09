namespace Paintball.Validators
{
    using Paintball.Abstractions.Constants;
    using Paintball.Abstractions.Exceptions;
    using Paintball.Abstractions.Validators;
    using Paintball.Database.Abstractions.Entities;

    public sealed class DuplicatesChecker : IDuplicatesChecker
    {
        public void CheckDuplicates(IList<GameResult> gameResults)
        {
            bool containsDuplicates = gameResults.GroupBy(result => result.Id)
                .Any(group => group.Count() > 1);

            if (containsDuplicates)
            {
                throw new DuplicatedRecordsException(ExceptionMessages.DuplicatedRecord);
            }
        }
    }
}