using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Middleware
{
    public class Program
    {
        private static IConfiguration KestrelConfig { get; } = new ConfigurationBuilder()
         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
         .AddJsonFile("appsettingsCustom.json", optional: false, reloadOnChange: false)
         .AddEnvironmentVariables()
         .Build();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
            .ConfigureLogging(log =>
            {
                log.AddConsole().SetMinimumLevel(LogLevel.Information);
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(opt =>
                    {
                        opt.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(10);
                        opt.Limits.MaxConcurrentConnections = 200_000;
                        opt.Limits.MaxConcurrentUpgradedConnections = 100;
                        opt.Limits.MaxRequestBodySize = 1024 * 2024 * 2; // 2MB

                    });

                    webBuilder.UseStartup<Startup>().UseConfiguration(KestrelConfig);
                });
    }
}
