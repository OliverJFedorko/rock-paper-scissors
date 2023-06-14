using Microsoft.EntityFrameworkCore;
using Rock_Paper_Scissors.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RockPaperScissors
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasKey(p => p.Name);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestDB;"); // Replace with your SQL Server connection string if you want to test :)
        }
    }
}
