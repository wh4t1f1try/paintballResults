#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Paintball.Database.Contexts;

namespace PaintballResults.Api.Migrations
{
    [DbContext(typeof(GameResultContext))]
    internal class GameResultContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("PaintballResults.Api.Models.GameResult", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<int>("Gameday")
                    .HasColumnType("INTEGER");

                b.Property<string>("TeamOne")
                    .HasColumnType("TEXT");

                b.Property<int>("TeamOneMatchPoints")
                    .HasColumnType("INTEGER");

                b.Property<string>("TeamTwo")
                    .HasColumnType("TEXT");

                b.Property<int>("TeamTwoMatchPoints")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.ToTable("GameResults");
            });
#pragma warning restore 612, 618
        }
    }
}