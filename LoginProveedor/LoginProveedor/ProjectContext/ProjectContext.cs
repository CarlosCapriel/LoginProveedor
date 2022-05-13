using LoginProveedor.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginProveedor.ProjectContext
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginAdmin>(eb =>
            {
                eb.HasKey(c => new { c.Id });
            });
        }

        public DbSet<LoginAdmin> LoginAdmin { get; set; }
    }
}
