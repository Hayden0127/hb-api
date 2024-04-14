using HB.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HB.Database
{
    public class HBContextFactory : IDesignTimeDbContextFactory<HBContext>
    {
        public IConfiguration _configuration { get; }

        public HBContext CreateDbContext(string[] args)
        {
            var connectionString = "Data Source=host.docker.internal,56338;Initial Catalog=NSRCC_Faci;User ID=admin;Password=Password123";

            var optionsBuilder = new DbContextOptionsBuilder<HBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new HBContext(optionsBuilder.Options);
        }
    }
}
