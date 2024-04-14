using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Strateq.Core.API.Infrastructures.EventBus;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Strateq.Core.Database;
using Strateq.Core.API.Extensions;
using HB.API.Extensions;
using Strateq.Core.Utilities;
using Strateq.Core.API.Infrastructures.EventBus.RabbitMQ;
using HB.Database;
using Strateq.Core.API.Infrastructures.EventBus.Abstractions;
using HB.Utilities;
using Strateq.Core.API.Infrastructures.EventBus.AzureServiceBus;
using Strateq.Core.API.Middlewares;



namespace HB.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HB.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. </br> 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      </br> Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var xmlFiles = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ""), "*.xml");
                foreach (var file in xmlFiles)
                {
                    c.IncludeXmlComments(file);
                }

                c.EnableAnnotations();
            });
            services.ConfigureMVC();
            services.ConfigureCoreRepositories();
            services.ConfigureCoreServices();
            //services.ConfigureCoreDatabase(Configuration);
            services.AddHttpContextAccessor();
            services.ConfigureServices(Configuration);
            services.ConfigureDatabase(Configuration);
            services.AddAutoMapper(typeof(AutoMappingCore));
            services.AddAutoMapper(typeof(AutoMapping));
            services.ConfigureRepositories();
            services.ConfigureAzureBlob(Configuration);

            //TODO
            //var key = Encoding.UTF8.GetBytes(Configuration["jwt-secret-key"]);
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt-Token"]);

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidAudience = gatewayType,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context => {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddControllers();

            services.AddCors(options => {
                options.AddDefaultPolicy(
                    builder => {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddCors(options => {
                options.AddPolicy("MyPolicy",
                    builder => {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            var busEnabled = Environment.GetEnvironmentVariable("SERVICEBUS_ENABLED") ?? "";

            if (busEnabled == "true")
            {
                RegisterEventBus(services);
            }
        }


        public ILifetimeScope? AutofacContainer { get; private set; }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            //builder.RegisterModule(new MyApplicationModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseSwagger(c => {
                    c.RouteTemplate = "/swagger/{documentname}/swagger.json";
                });
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "cpms API V1.1");
                });
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction())
            {
                string prefixBasepath = "/cpms";
                app.UseSwagger(c => {
                    c.RouteTemplate = "/swagger/{documentname}/swagger.json";
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{prefixBasepath}" } };
                    });
                });
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/cpms/swagger/v1/swagger.json", "cpms.API V1.1");
                });
            }

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseCors("MyPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseSession();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers().RequireCors("MyPolicy");
            });

            var migrationEnabled = Environment.GetEnvironmentVariable("DATABASE_MIGRATION_ENABLED") ?? "";

            if (migrationEnabled == "true")
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<LoggingContext>();
                    context.Database.Migrate();
                }

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<HBContext>();
                    context.Database.Migrate();
                }
            }

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            var busEnabled = Environment.GetEnvironmentVariable("SERVICEBUS_ENABLED") ?? "";

            if (busEnabled == "true")
            {
                ConfigureEventBus(app);
            }
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionName = Configuration["SubscriptionClientName"];
            var serviceBusConnectionString = Environment.GetEnvironmentVariable("SERVICEBUS_CONNECTION_STRING") ?? "";
            var busType = Environment.GetEnvironmentVariable("BUS_TYPE") ?? "";


            if (!string.IsNullOrEmpty(busType) && busType.ToLower() == "azure")
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp => {
                    return new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
                });

                services.AddSingleton<IEventBus, EventBusServiceBus>(sp => {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    //var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                    var iLifetimeScope = AutofacContainer;
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, iLifetimeScope, subscriptionName);
                });
            }
            else
            {
                services.AddSingleton<IRabbitMQPersisterConnection>(sp => {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersisterConnection>>();
                    var factory = new ConnectionFactory();
                    factory.DispatchConsumersAsync = true;
                    factory.HostName = Configuration["RabbitMQConnection"];
                    //factory.Port = Int32.Parse(Configuration["RabbitMQPort"]);
                    factory.UserName = Configuration["RabbitMQUserName"];
                    factory.Password = Configuration["RabbitMQPassword"];
                    return new DefaultRabbitMQPersisterConnection(factory, logger);
                });

                services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp => {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IRabbitMQPersisterConnection>();
                    //var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                    var iLifetimeScope = AutofacContainer;
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusRabbitMQ(serviceBusPersisterConnection, logger,
                            iLifetimeScope, eventBusSubcriptionsManager, subscriptionName);
                });
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
