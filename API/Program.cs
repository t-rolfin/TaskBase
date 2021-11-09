using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskBase.Data;
using TaskBase.Data.Identity;
using TaskBase.Data.Utils;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();
        var log = LoggerFactory.Create(x => x.AddLog4Net())
            .CreateLogger<Startup>();

        try
        {
            log.LogInformation("Application Programing Interface is starting...");

            var host = CreateWebHostBuilder(args)
                .ConfigureAppConfiguration(config => GetConfiguration()).Build();

            host.MigrateDbContext<IdentityContext>((context, services) =>
            {
                var env = (IWebHostEnvironment)services.GetService(typeof(IWebHostEnvironment));
                var logger = (ILogger<IdentityContextSeed>)services.GetService(typeof(ILogger<IdentityContextSeed>));

                new IdentityContextSeed()
                .SeedAsync(context, env, logger)
                .Wait();
            });

            host.MigrateDbContext<TaskContext>((context, services) =>
            {
                var identityContext = (IdentityContext)services.GetService(typeof(IdentityContext));

                new TaskContextSeed()
                .SeedAsync(identityContext, context)
                .Wait();
            });

            host.Run();
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    => WebHost.CreateDefaultBuilder(args)
    .CaptureStartupErrors(false)
    .UseStartup<Startup>()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureLogging(x =>
    {
        x.ClearProviders();
        x.AddLog4Net();
    });

    public static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder();

        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}





