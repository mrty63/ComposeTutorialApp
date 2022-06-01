using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.Contrib.Simmy.Behavior;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;


using Polly.Extensions.Http;
using Polly.Contrib.WaitAndRetry;
using Polly.Contrib.Simmy.Utilities;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            long fileSizeLim = 1000000;
            var isEnabled = true;
            var chaosPolicy = MonkeyPolicy.InjectLatency(with =>
                with.Latency(TimeSpan.FromSeconds(5))
                    .InjectionRate(1)
                    .Enabled(isEnabled)
                );
            var chaosPolicy2 = MonkeyPolicy.InjectBehaviour(with =>
                with.Behaviour(() => Log.Debug("Chaos Policy two hit"))
                    .InjectionRate(1)
                    .Enabled(isEnabled)
                );

            var path = "webAppLog-.txt";
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Debug()
            .WriteTo.File(path, LogEventLevel.Verbose, rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeLim, rollingInterval: RollingInterval.Day, retainedFileTimeLimit: TimeSpan.FromDays(5))
            .WriteTo.Console(LogEventLevel.Debug)
            .CreateLogger();

            try
            {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
