using Microsoft.EntityFrameworkCore;
using PocketFlowSharpGallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketFlowSharpGallery.Data
{
    public class PFSDbContext : DbContext
    {
        public PFSDbContext(DbContextOptions<PFSDbContext> options)
            : base(options)
        {
        }

        public DbSet<LLMConfig> LLMConfigs { get; set; }
        public DbSet<SearchEngineConfig> SearchEngineConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LLMConfig>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Provider).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EndPoint).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ModelName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ApiKey).IsRequired().HasMaxLength(1000);
            });

            modelBuilder.Entity<SearchEngineConfig>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Provider).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EndPoint).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ApiKey).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Description).HasMaxLength(500);
            });
        }
    }
}
