using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.RetryPolicies.Config
{
    public interface ICircuitBreakerPolicyConfig
    {
        int RetryCount { get; set; }
        int BreakDuration { get; set; }
    }

    public interface IRetryPolicyConfig
    {
        int RetryCount { get; set; }
    }

    public class PolicyConfig : ICircuitBreakerPolicyConfig, IRetryPolicyConfig
    {
        public int RetryCount { get; set; }
        public int BreakDuration { get; set; }
    }
}