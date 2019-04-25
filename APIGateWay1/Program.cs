using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Ocelot.Provider.Polly;
using Microsoft.Extensions.Logging;
using NLog.Web;
using CacheManager.Core.Logging;
using NLog;

namespace APIGateWay1
{
    public class Program
    {
        private static NLog.ILogger _logger;
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        [System.Obsolete]
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("ocelot.json")
                .AddEnvironmentVariables();
            })
            .ConfigureServices((builderContext, services) =>
            {
                services.AddOcelot()
                .AddPolly();
            })
            .Configure(app =>
            {
                app.UseOcelot().Wait();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                logging.AddConsole();
            })
            .UseIISIntegration()
            .Build();
        }       
    }
}
