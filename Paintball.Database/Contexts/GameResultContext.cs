using Microsoft.EntityFrameworkCore;
using Paintball.Database.Abstractions.Entities;

namespace Paintball.Database.Contexts
{
    public class GameResultContext : DbContext
    {
        public GameResultContext(DbContextOptions<GameResultContext> options) : base(options)
        {
        }

        public DbSet<GameResult> Gameresults => Set<GameResult>();
    }
}