using Microsoft.EntityFrameworkCore;
using System;
using Strateq.Core.Database.DbModel;
//using System.Data.Entity;

namespace Strateq.Core.Database
{
    public partial class LoggingContext : DbContext
    {
        public DbSet<SystemLog> SystemLog { get; set; }
        public DbSet<RequestLog> RequestLog { get; set; }

        public LoggingContext(DbContextOptions<LoggingContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cpms");
        }
    }
}
