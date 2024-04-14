using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Strateq.Core.Database;
using HB.Database;
using HB.Database.UnitOfWork;
using HB.Database.Repositories;
using Azure.Storage.Blobs;
using HB.Service;

namespace HB.API.Extensions
{
    public static partial class ServiceExtensions
    {

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSession(options => {
                // 20 minutes later from last access your session will be removed.
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });

            //var redisConnection = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "";

            //services.AddStackExchangeRedisCache(options => {
            //    options.Configuration = redisConnection;
            //});

            services.AddTransient<IOnBoardingService, OnBoardingService>();
            services.AddTransient<ICPMonitoringService, CPMonitoringService>();
            services.AddTransient<ICPBreakdownErrorService, CPBreakdownErrorService>();
            services.AddTransient<ICPTransactionService, CPTransactionService>();
            services.AddTransient<IRunningSequenceService, RunningSequenceService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IProductTypeService, ProductTypeService>();
            services.AddTransient<ICPPricingService, CPPricingService>();
            services.AddTransient<IBookingService, BookingService>();
        }

        public static void ConfigureAzureBlob(this IServiceCollection services, IConfiguration configuration)
        {
            var blobConnectionString = Environment.GetEnvironmentVariable("AZUREBLOBSTORAGE_CONNECTION_STRING") ?? "";
            services.AddScoped(x => new BlobServiceClient(blobConnectionString));
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? "";

            services.AddDbContext<HBContext>(options =>
            options.UseSqlServer(connectionString,
            c => {
                c.MigrationsAssembly("HB.Database");
                c.MigrationsHistoryTable("_MigrationHistory", "cpms");
            }
            ));

            services.AddDbContext<LoggingContext>(options =>
            options.UseSqlServer(connectionString,
            c => {
                c.MigrationsAssembly("Strateq.Core.Database");
                c.MigrationsHistoryTable("_MigrationHistory", "cpms");
            }
            ));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICPSiteDetailsRepository, CPSiteDetailsRepository>();
            services.AddTransient<ICPDetailsRepository, CPDetailsRepository>();
            services.AddTransient<ICPConnectorRepository, CPConnectorRepository>();
            services.AddTransient<ICPBreakdownErrorRepository, CPBreakdownErrorRepository>();
            services.AddTransient<ICPTransactionRepository, CPTransactionRepository>();
            services.AddTransient<IRunningSequenceRepository,RunningSequenceRepository>();
            services.AddTransient<IUserAccountRepository, UserAccountRepository>();
            services.AddTransient<IUserAccountAuthorizationTokenRepository, UserAccountAuthorizationTokenRepository>();
            services.AddTransient<ICPBreakdownDurationDetailsRepository, CPBreakdownDurationDetailsRepository>();
            services.AddTransient<IPriceVariesRepository, PriceVariesRepository>();
            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
            services.AddTransient<IUnitRepository, UnitRepository>();
            services.AddTransient<IPricingPlanRepository, PricingPlanRepository>();
            services.AddTransient<IPricingPlanTypeRepository, PricingPlanTypeRepository>();
            services.AddTransient<IMeterValueRepository, MeterValueRepository>();
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IGuestDetailsRepository, GuestDetailsRepository>();
            services.AddTransient<IPaymentDetailsRepository, PaymentDetailsRepository>();
        }
    }
}
