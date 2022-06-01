using Polly.Extensions.Http;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp;
using System.Text.Json;
using WebApp.Chaos.Extensions;
using Microsoft.Extensions.Configuration;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Http.Polly;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Contrib.Simmy;
using Polly.Registry;
using Polly.Contrib.Simmy.Behavior;
using Polly.Contrib.Simmy.Latency;
using Polly.Contrib.Simmy.Outcomes;
using Microsoft.Extensions;
namespace WebApp.Chaos
{
    public class ChaosApiHttpClient
    {
        //private readonly ResilientHttpClient _client;
        private readonly IConfiguration _config;

        public ChaosApiHttpClient(/*ResilientHttpClient client,*/ IConfiguration configuration)
        {
            //_client = client ?? throw new ArgumentNullException(nameof(client));
            _config = configuration;
        }

        public Task<AppChaosSettings> GetGeneralChaosSettings()
        {
            //var response = await _client.GetAsync("/api/chaos/get");
            //if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            //    return new GeneralChaosSetting { OperationChaosSettings = new List<OperationChaosSetting>(), ExecutionInformation = new ExecutionInformation() };

            //return JsonSerializer.Deserialize<GeneralChaosSetting>(await response.Content.ReadAsStringAsync());
            AppChaosSettings chaosSettings = new AppChaosSettings();
           _config.GetSection("ChaosSettings").Bind(chaosSettings);

            return Task.FromResult(chaosSettings);


        }

        //public async Task UpdateGeneralChaosSettings(GeneralChaosSetting settings)
        //{
        //    var response = await _client.PostAsync("/api/chaos/update", new JsonContent(settings));
        //    response.EnsureSuccessStatusCode();
        //}
    }
}
