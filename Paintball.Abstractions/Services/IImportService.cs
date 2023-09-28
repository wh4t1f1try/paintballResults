namespace Paintball.Abstractions.Services;

public interface IImportService
{
    void ImportGameResults(Stream? stream);
}