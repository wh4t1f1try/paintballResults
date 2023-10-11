namespace Paintball.Abstractions.Exceptions
{
    using System.Diagnostics.CodeAnalysis;

    public class InvalidArgumentException : Exception
    {
        [ExcludeFromCodeCoverage]
        public InvalidArgumentException(string? message)
            : base(message)
        {
        }
    }
}