using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;
using Paintball.Abstractions.Validators;
using Paintball.Database.Abstractions.Entities;

namespace Paintball.Validators
{
    public sealed class DuplicatesChecker : IDuplicatesChecker
    {
        public void CheckDuplicates(IList<GameResult> gameResults)
        {
            var containsDuplicates = gameResults.GroupBy(result => result.Id)
                .Any(group => group.Count() > 1);

            if (containsDuplicates)
            {
                throw new DuplicatedRecordsException(ExceptionMessages.DuplicatedRecord);
            }
        }
    }
}