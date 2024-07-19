using DevProdWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System.Data;
using System.Reflection;

namespace DevProdWebApp.Utilities
{
        public class AppDbContext : DbContext
        {

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Models.Project> Projects { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ToolMetric> ToolMetric { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }


            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            }
        }
    
}
