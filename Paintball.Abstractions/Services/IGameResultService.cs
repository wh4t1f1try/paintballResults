using Paintball.Database.Abstractions.Entities;

namespace Paintball.Abstractions.Services
{
    public interface IGameResultService
    {
        IList<GameResult> GetAll();

        GameResult GetById(int id);

        IList<GameResult> GetByName(string teamName);

        void Delete();
    }
}