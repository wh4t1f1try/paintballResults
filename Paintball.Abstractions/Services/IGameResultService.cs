namespace Paintball.Abstractions.Services
{
    using Paintball.Database.Abstractions.Entities;

    public interface IGameResultService
    {
        IList<GameResult> GetAll();

        GameResult GetById(int id);

        IList<GameResult> GetByName(string teamName);
    }
}