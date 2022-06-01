using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace WebApp
{
    public class ResilientHttpClient
    {
        private readonly HttpClient m_client;

        public ResilientHttpClient(HttpClient client)
        {
            this.m_client = client;
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, Context context)
        {
            //string results = new string { Results = new List<string>() };

            // In a real app, would use a Task.WhenAll() fanout pattern.
            var response = await GetAsyncUsingContext(uri, context);
                //results.Results.Add(new EndpointResult() { Url = endpoint, Value = (int)response.StatusCode });
            

            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, Context context)
        {
            // We attach the Polly context to the HttpRequestMessage using an extension method provided by HttpClientFactory.
            message.SetPolicyExecutionContext(context);

            // Make the request using the client configured by HttpClientFactory, which embeds the Polly and Simmy policies.
            return await m_client.SendAsync(message);
        }

        public async Task<HttpResponseMessage> GetAsyncUsingContext(string url, Context context)
        {
            // This will include configured Polly resilience policies; and Simmy chaos policies in dev environments.
            // - Polly resilience policies were configured in StartUp
            // - A call to .AddChaosInjectors() added chaos policies to all policies in the registry, during startup, for dev environments.

            // We attach the Polly context to the HttpRequestMessage using an extension method provided by HttpClientFactory.
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
            message.SetPolicyExecutionContext(context);

            // Make the request using the client configured by HttpClientFactory, which embeds the Polly and Simmy policies.
            return await m_client.SendAsync(message);
        }
    }
}
