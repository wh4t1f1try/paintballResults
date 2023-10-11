namespace Paintball.Abstractions.Services
{
    using Paintball.Abstractions.DTOs;

    public interface IGameResultService
    {
        IList<GameResultDto> GetAll();

        GameResultDto GetById(int id);

        IList<GameResultDto> GetByName(string teamName);
    }
}