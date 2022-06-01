using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Polly.Contrib.WaitAndRetry;

namespace WebApp
{
    public static class HttpClientBuilderExtensions
    {
        public static void AddPolicyHandlerApi(
            this IHttpClientBuilder httpClientBuilder,
            IConfiguration configuration)
        {
            var enableChaosEngineering = configuration.GetValue<bool>("ChaosEngineering:Enable", true);
            var chaosInjectionRate = configuration.GetValue<double>("ChaosEngineering:InjectionRate", 0.75);

            httpClientBuilder.AddPolicyHandlerApi(enableChaosEngineering, chaosInjectionRate);
        }
   
        public static void AddPolicyHandlerApi(
                this IHttpClientBuilder httpClientBuilder,
                bool enableChaosEngineering = true,
                double chaosInjectionRate = 0.75)
        {
            httpClientBuilder.AddTransientHttpErrorPolicyHandler();

            if (enableChaosEngineering)
            {
                httpClientBuilder.AddTransientHttpErrorMonkeyPolicyHandler(chaosInjectionRate);
            }

        }
        public static void AddTransientHttpErrorPolicyHandler(
                this IHttpClientBuilder httpClientBuilder)
        {
            // https://github.com/Polly-Contrib/Polly.Contrib.WaitAndRetry#wait-and-retry-with-jittered-back-off
            // Jitter causes each request to vary slightly on retry, which decorrelates the retries from each other.
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 3);

            httpClientBuilder.AddPolicyHandler((services, request) => HttpPolicyExtensions.HandleTransientHttpError()
                    .WaitAndRetryAsync(delay,
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        Log.Warning($"Delaying for {timespan.TotalMilliseconds}ms, then making retry {retryAttempt}.", timespan.TotalMilliseconds, retryAttempt);
                    }
                ));

        }
        public static void AddTransientHttpErrorMonkeyPolicyHandler(
                this IHttpClientBuilder httpClientBuilder,
                double chaosInjectionRate = 0.75) // 0 .. 1, chance of firing
        {
            // Simmy
            var httpNotFoundResult = new HttpResponseMessage(HttpStatusCode.NotFound);
            var httpNotFoundPolicy = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with =>
                with.Result(httpNotFoundResult)
                    .InjectionRate(chaosInjectionRate)
                    .Enabled()
            );

            var internalServerErrorResult = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var internalServerErrorPolicy = MonkeyPolicy.InjectResultAsync<HttpResponseMessage>(with =>
                with.Result(internalServerErrorResult)
                    .InjectionRate(chaosInjectionRate)
                    .Enabled()
            );

            // Timeout and return to the caller after 2 seconds, if the executed delegate has not completed.  Optimistic timeout: Delegates should take and honour a CancellationToken.
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(20000));
            var policies = Policy.WrapAsync<HttpResponseMessage>(timeoutPolicy, httpNotFoundPolicy, internalServerErrorPolicy);

            httpClientBuilder.AddPolicyHandler(timeoutPolicy);

        }
}
}