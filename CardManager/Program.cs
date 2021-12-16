using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Data;
using System;

namespace CardManager
{
    public class Program
    {
        /// <summary>
        /// Options Kestrel.
        /// </summary>
        private static IConfiguration KestrelConfig { get; } = new ConfigurationBuilder()
         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
         .AddJsonFile("KestrelOptions.json", optional: true, reloadOnChange: true)
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
                log.AddDebug().SetMinimumLevel(LogLevel.Information);
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(opt =>
                    {
                        KestrelOptions Kestrel = KestrelConfig.Get<KestrelOptions>();
                        opt.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(Kestrel.KeepAliveTimeout);
                        opt.Limits.MaxConcurrentConnections = Kestrel.MaxConcurrentConnections;
                        opt.Limits.MaxConcurrentUpgradedConnections = Kestrel.MaxConcurrentUpgradedConnections;
                        opt.Limits.MaxRequestBodySize = Kestrel.MaxRequestBodySize;
                    });

                    webBuilder.UseStartup<Startup>().UseConfiguration(KestrelConfig);
                });
    }
}
