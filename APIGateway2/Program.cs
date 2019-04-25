using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using NLog.Web;

namespace APIGateWay1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        [Obsolete]
        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .UseStartup<Startup>()
                          .ConfigureAppConfiguration((hostingContext, config) =>
                          {
                              config.AddJsonFile("ocelot.json")
                              .AddEnvironmentVariables();
                          })
                          .ConfigureServices(s =>
                          {
                              s.AddOcelot();
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
