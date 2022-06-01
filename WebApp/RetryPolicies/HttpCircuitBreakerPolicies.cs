using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Polly;
using Polly.CircuitBreaker;
using System.Net.Http;
using WebApp.RetryPolicies.Config;

namespace WebApp.RetryPolicies
{
    public class HttpCircuitBreakerPolicies
    {
        public static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(ILogger logger, ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
        {
            return HttpPolicyBuilders.GetBaseBuilder()
                                      .CircuitBreakerAsync(circuitBreakerPolicyConfig.RetryCount + 1,
                                                           TimeSpan.FromSeconds(circuitBreakerPolicyConfig.BreakDuration),
                                                           (result, breakDuration) =>
                                                           {
                                                               OnHttpBreak(result, breakDuration, circuitBreakerPolicyConfig.RetryCount, logger);
                                                           },
                                                           () =>
                                                           {
                                                               OnHttpReset(logger);
                                                           });
        }

        public static void OnHttpBreak(DelegateResult<HttpResponseMessage> result, TimeSpan breakDuration, int retryCount, ILogger logger)
        {
            Log.Error("Service shutdown during {breakDuration} after {DefaultRetryCount} failed retries.", breakDuration, retryCount);
            throw new BrokenCircuitException("Service inoperative. Please try again later");
        }

        public static void OnHttpReset(ILogger logger)
        {
            Log.Information("Service restarted.");
        }
    }
}
