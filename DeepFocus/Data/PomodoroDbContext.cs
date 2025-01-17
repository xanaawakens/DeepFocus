using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DeepFocus.Models;
using Windows.Storage;

namespace DeepFocus.Data
{
    public class PomodoroDbContext : DbContext
    {
        public DbSet<PomodoroSession> Sessions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pomodoro.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PomodoroSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.SessionType).IsRequired();
                entity.Property(e => e.IsCompleted).IsRequired();
            });
        }
    }
}
