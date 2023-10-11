namespace Paintball.Database.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using Paintball.Database.Abstractions.Entities;

    public class GameResultContext : DbContext
    {
        public GameResultContext(DbContextOptions<GameResultContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=mydatabase.db");
        //}

        public DbSet<GameResult> GameResults => this.Set<GameResult>();
    }
}