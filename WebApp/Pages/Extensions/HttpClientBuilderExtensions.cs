using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
//using Microsoft.Extensions.Logging;
using WebApp.RetryPolicies;
using WebApp.RetryPolicies.Config;

namespace WebApp.Extensions
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddPolicyHandlers(this IHttpClientBuilder httpClientBuilder, string policySectionName, ILogger loggerFactory, IConfiguration configuration)
        {
            var retryLogger = loggerFactory;
            var circuitBreakerLogger = loggerFactory;

            var policyConfig = new PolicyConfig();
            configuration.Bind(policySectionName, policyConfig);

            var circuitBreakerPolicyConfig = (ICircuitBreakerPolicyConfig)policyConfig;
            var retryPolicyConfig = (IRetryPolicyConfig)policyConfig;

            return httpClientBuilder.AddRetryPolicyHandler(retryLogger, retryPolicyConfig)
                                    .AddCircuitBreakerHandler(circuitBreakerLogger, circuitBreakerPolicyConfig);
        }

        public static IHttpClientBuilder AddRetryPolicyHandler(this IHttpClientBuilder httpClientBuilder, ILogger logger, IRetryPolicyConfig retryPolicyConfig)
        {
            return httpClientBuilder.AddPolicyHandler(HttpRetryPolicies.GetHttpRetryPolicy(logger, retryPolicyConfig));
        }

        public static IHttpClientBuilder AddCircuitBreakerHandler(this IHttpClientBuilder httpClientBuilder, ILogger logger, ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
        {
            return httpClientBuilder.AddPolicyHandler(HttpCircuitBreakerPolicies.GetHttpCircuitBreakerPolicy(logger, circuitBreakerPolicyConfig));
        }
    }
}