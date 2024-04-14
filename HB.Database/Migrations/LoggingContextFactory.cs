using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Strateq.Core.Database;

namespace HB.Database.Migrations
{
    public class LoggingContext2Factory : IDesignTimeDbContextFactory<LoggingContext>
    {
        public IConfiguration _configuration { get; }

        public LoggingContext CreateDbContext(string[] args)
        {
            var connectionString = "Data Source=localhost,1399;Initial Catalog=NSRCC;User ID=sa;Password=1212;";

            var optionsBuilder = new DbContextOptionsBuilder<LoggingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LoggingContext(optionsBuilder.Options);
        }
    }
}
