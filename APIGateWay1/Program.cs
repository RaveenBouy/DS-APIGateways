using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Ocelot.Provider.Polly;
using Microsoft.Extensions.Logging;

namespace APIGateWay1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

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
                s.AddOcelot()
                .AddPolly();
            })
            .Configure(app =>
            {
                app.UseOcelot().Wait();
            })
            .ConfigureLogging((hostingContext, logging) =>
            {

            })
            .UseIISIntegration()
            .Build();
        }       
    }
}
