using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace WebApp.Extensions
{
	public class HttpClientService :  IHttpClientService
	{
		private readonly HttpClient _client;
		private readonly IConfiguration _configuration;
		//private readonly JsonSerializerSettings _serializerSettings;
		public HttpClientService(HttpClient client, IConfiguration configuration) 
		{
			

				client.DefaultRequestHeaders.Add("Accept", "application/json");
				_client = client;
				_configuration = configuration;

				////_serializerSettings = new JsonSerializerSettings
				//{
				//	ContractResolver = new DefaultContractResolver
				//	{
				//		NamingStrategy = new SnakeCaseNamingStrategy()
				//	}
				//};
			}
		async Task<HttpResponseMessage> IHttpClientService.GetAsync(string uri)
		{
			//string results = new string { Results = new List<string>() };

			// In a real app, would use a Task.WhenAll() fanout pattern.
			var response = await _client.GetAsync(uri);
			//results.Results.Add(new EndpointResult() { Url = endpoint, Value = (int)response.StatusCode });


			return response;
		}

        
    }
	
}
