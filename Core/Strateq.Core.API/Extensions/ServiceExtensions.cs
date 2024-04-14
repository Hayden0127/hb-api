
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Strateq.Core.API.Filter;
using Strateq.Core.Database;
using Strateq.Core.Database.Repositories;
using Strateq.Core.Service;

namespace Strateq.Core.API.Extensions
{
    public static partial class ServiceExtensions
    {
        public static void ConfigureMVC(this IServiceCollection services)
        {
            services.AddMvcCore(options => {
                options.Filters.Add(typeof(ModelStateValidationFilter));
            });
        }

        public static void ConfigureCoreRepositories(this IServiceCollection services)
        {
            services.AddTransient<ISystemLogRepository, SystemLogRepository>();
            services.AddTransient<IRequestLogRepository, RequestLogRepository>();
        }

        public static void ConfigureCoreServices(this IServiceCollection services)
        {
            services.AddTransient<ISystemLogService, SystemLogService>();
            services.AddTransient<IRequestLogService, RequestLogService>();
            //services.AddTransient<IRedisCacheService, RedisCacheService>();
            services.AddTransient<IPermissionService, PermissionService>();
        }

        public static void ConfigureCoreDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LoggingContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
        }
    }
}
