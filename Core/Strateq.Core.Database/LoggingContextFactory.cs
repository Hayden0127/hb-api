using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Strateq.Core.Database
{
    public class LoggingContextFactory : IDesignTimeDbContextFactory<LoggingContext>
    {
        public IConfiguration _configuration { get; }

        public LoggingContext CreateDbContext(string[] args)
        {
            var connectionString = "Data Source=host.docker.internal,1433;Initial Catalog=NSRCC;User ID=sa;Password=1212;";

            var optionsBuilder = new DbContextOptionsBuilder<LoggingContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LoggingContext(optionsBuilder.Options);
        }
    }
}
