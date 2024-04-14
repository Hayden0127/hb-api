using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration((context, config) => {
                    var settings = config.Build();

                    var configValue = settings.GetValue<string>("ENV") ?? String.Empty;

                    //if (configValue.ToLower() == "stg" || configValue.ToLower() == "prod")
                    //{
                    //    // Way-1
                    //    // Connect to Azure Key Vault using the Managed Identity.
                    //    //var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];
                    //    var keyVaultEndpoint = settings.GetValue<string>("AZUREKEYVAULT_ENDPOINT");

                    //    if (!string.IsNullOrEmpty(keyVaultEndpoint))
                    //    {
                    //        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    //        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                    //        config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                    //    }
                    //}
                    //else
                    //{
                    //    //Way - 2
                    //    //Connect to Azure Key Vault using the Client Id and Client Secret(AAD) -Get them from Azure AD Application.
                    //    var keyVaultEndpoint = settings["AZUREKEYVAULT_ENDPOINT"];
                    //    var keyVaultClientId = settings["AZUREKEYVAULT_CLIENTID"];
                    //    var keyVaultClientSecret = settings["AZUREKEYVAULT_CLIENTSECRET"];

                    //    if (!string.IsNullOrEmpty(keyVaultEndpoint) && !string.IsNullOrEmpty(keyVaultClientId) && !string.IsNullOrEmpty(keyVaultClientSecret))
                    //    {
                    //        config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClientId, keyVaultClientSecret, new DefaultKeyVaultSecretManager());
                    //    }
                    //}
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
