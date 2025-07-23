using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
