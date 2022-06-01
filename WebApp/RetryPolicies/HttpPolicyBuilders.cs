using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Net.Http;
namespace WebApp.RetryPolicies
{
    public static class HttpPolicyBuilders
    {
        public static PolicyBuilder<HttpResponseMessage> GetBaseBuilder()
        {
            return HttpPolicyExtensions.HandleTransientHttpError();
        }
    }
}
