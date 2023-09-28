namespace Paintball.Abstractions.Exceptions
{
    public class FileHasNoDataException : Exception
    {
        public FileHasNoDataException(string? message) : base(message)
        {
        }
    }
}