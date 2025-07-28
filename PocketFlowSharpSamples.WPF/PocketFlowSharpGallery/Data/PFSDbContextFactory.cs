using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace PocketFlowSharpGallery.Data
{
    public class PFSDbContextFactory : IDesignTimeDbContextFactory<PFSDbContext>
    {
        public PFSDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PFSDbContext>();
            
            // 使用与App.xaml.cs中相同的数据库路径逻辑
            var projectRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
            projectRootPath = Path.GetFullPath(projectRootPath);
            var dbPath = Path.Combine(projectRootPath, "Data", "pocketflowcharp.db");
            
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new PFSDbContext(optionsBuilder.Options);
        }
    }
} 