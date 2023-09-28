using System.Text;
using Paintball.Abstractions.Constants;
using Paintball.Abstractions.Exceptions;

namespace PaintballResults.Generator
{
    public sealed class TestRecordsGenerator
    {
        private string GenerateRandomString(int start, int end)
        {
            if (start < 0 || end < 0 || start > end)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidArgument);
            }

            var random = new Random();
            StringBuilder randomString = new();

            var randomLength = random.Next(start, end);

            var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÜÖÄüöäß";


            for (var i = 0; i < randomLength; i++)
            {
                randomString.Append(letters[random.Next(letters.Length)]);
            }

            return randomString.ToString();
        }


        private string[] CreateRecordWithInvalidTestItem(string testItem)
        {
            string[] recordDummy = { "Spiel", "Tag", "Team 1", "Team 2", "T1 MP", "T2 MP" };

            Random random = new();
            var randomIndex = random.Next(0, recordDummy.Length);

            recordDummy[randomIndex] = testItem;

            return recordDummy;
        }

        private static IEnumerable<string[]> CreateInvalidTestRecords()
        {
            yield return new[] { "a", "1", "Wanderers Bremen", "Lucky Bastards", "0", "4" };
            yield return new[] { "1", "a", "Wanderers Bremen", "Lucky Bastards", "0", "4" };
            yield return new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "a", "4" };
            yield return new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "0", "a" };
        }

        private static IEnumerable<string[]> CreateValidTestRecords()
        {
            yield return new[] { "1", "1", "Wanderers Bremen", "Lucky Bastards", "1", "4" };
            yield return new[] { "2", "2", "Wanderers Bremen", "Lucky Bastards", "2", "3" };
            yield return new[] { "3", "3", "Wanderers Bremen", "Lucky Bastards", "3", "2" };
            yield return new[] { "4", "4", "Wanderers Bremen", "Lucky Bastards", "4", "1" };
        }

        private static IEnumerable<string[]> CreateValidDataStrings()
        {
            yield return new[] { "1; 1; Wanderers Bremen; Lucky Bastards; 0; 4 " };
            yield return new[] { "1; 2; Wanderers Bremen; Lucky Bastards; 0; 4 " };
            yield return new[] { "1; 1; Wanderers Bremen; Lucky Bastards; 3; 4 " };
            yield return new[] { "1; 1; Wanderers Bremen; Lucky Bastards; 0; 3 " };
        }
    }
}