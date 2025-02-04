using KeyChordFinder.Data;
using Microsoft.EntityFrameworkCore;
using PlatiKrab.Data.Models;

namespace PlatiKrab.Data
{
    public class PlatiKrabDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<PlayerTraining> PlayerTrainings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        private static readonly string ConnectionString = GetConnectionString("PlatiKrab.db");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Training)
                .WithOne(t => t.Payment)
                .HasForeignKey<Payment>(p => p.TrainingId); 
        }

        private static string GetConnectionString(string dbName)
        {
            var dbPath = DbHelper.GetDbPath(dbName);
            return $"Data source={dbPath}";
        }
    }
}
