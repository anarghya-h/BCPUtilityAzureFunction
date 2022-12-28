using BCPUtilityAzureFunction.Models.Configs;
using BCPUtilityAzureFunction.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BCPUtilityAzureFunction.Services;

[assembly: FunctionsStartup(typeof(BCPUtilityAzureFunction.Startup))]
namespace BCPUtilityAzureFunction
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Obtaining the configuration
            var Configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
           
            //Reading the connection string for the database
            string ConnString = Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<BCPUtilityDBContext>(
              options => options.UseSqlServer(ConnString));

            //Getting the base path
            var local_root = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azure_root = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actual_root = local_root ?? azure_root;
            //Creating a config to retrieve SDx Config data
            var config = new ConfigurationBuilder()
                            .SetBasePath(actual_root)
                            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

            SdxConfig sDxConfig = new SdxConfig();
            config.Bind("SdxConfig", sDxConfig);
            builder.Services.AddSingleton(sDxConfig);
            builder.Services.AddAutoMapper(typeof(SdxConfig));

            StorageAccountConfig Config = new StorageAccountConfig();
            config.Bind("StorageAccountConfig", Config);
            builder.Services.AddSingleton(Config);
            builder.Services.AddAutoMapper(typeof(StorageAccountConfig));

            /*WorkerConfig workerConfig = new WorkerConfig();
            config.Bind("WorkerConfig", workerConfig);
            builder.Services.AddSingleton(workerConfig);
            builder.Services.AddAutoMapper(typeof(WorkerConfig));*/

            //builder.Services.AddSingleton<IHostedService, AuthenticationService>(serviceProvider => serviceProvider.GetService<AuthenticationService>());
            //builder.Services.AddHostedService<AuthenticationService>();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.AzureTableStorageWithProperties(connectionString: Config.ConnectionString, Serilog.Events.LogEventLevel.Information, storageTableName: Config.TableName, propertyColumns:Config.propertyColumns)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Services.AddSingleton(Log.Logger);
            builder.Services.AddLogging(log =>
            {
                log.AddSerilog(Log.Logger);
            });

        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //base.ConfigureAppConfiguration(builder);
            //Getting the base path
            var local_root = Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot");
            var azure_root = $"{Environment.GetEnvironmentVariable("HOME")}/site/wwwroot";

            var actual_root = local_root ?? azure_root;
                            
            var builtConfig = builder.ConfigurationBuilder.SetBasePath(actual_root).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .Build();

            SdxConfig sDxConfig = new SdxConfig();
            builtConfig.Bind("SDxConfig", sDxConfig);
            string key = sDxConfig.AzureKeyVaultName;
            var secretClient = new SecretClient(
                            new Uri($"https://{builtConfig["SDxConfig:AzureKeyVaultName"]}.vault.azure.net/"),
                            new DefaultAzureCredential());
            builder.ConfigurationBuilder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            //builder.ConfigurationBuilder.AddUserSecrets(SDxConfig);
        }
    }
}
