namespace Paintball.Abstractions.Exceptions
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class InvalidFileException : Exception
    {
        public InvalidFileException(string? message)
            : base(message)
        {
        }
    }
}