using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.HealthChecks
{
    public class JobHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory m_clientFactory;
        public JobHealthCheck(IHttpClientFactory clientFactory)
        {

            m_clientFactory = clientFactory;
        }
        public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
        {
            //var healthCheckResultHealthy = true;
            var client = m_clientFactory.CreateClient();
            var pingResponse = client.GetAsync($"http://webapi/Jobs/Ping/");
            var statusStr = "Job Controller is ";
            if (pingResponse.Result.IsSuccessStatusCode)
            {
                var res1 = $"{statusStr}Healthy";
                Log.Information(res1);
                return Task.FromResult(
                    HealthCheckResult.Healthy(res1));
            }
            var res2 = $"{statusStr}UnHealthy";
            Log.Information(res2);
            return Task.FromResult(
                new HealthCheckResult(context.Registration.FailureStatus,res2));
        }
    }
}
