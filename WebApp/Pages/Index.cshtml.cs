using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Chaos;
using WebApp.Extensions;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> m_logger;
        private readonly IHttpClientFactory m_clientFactory;
        private readonly AppChaosSettings m_chaosSettings;
        private readonly ResilientHttpClient m_r_client;
        private readonly Task<AppChaosSettings> m_generalChaosSettingFactory;
        private readonly Uri m_jobPingUri;
        private readonly Uri m_skillsPingUri;
        private readonly IHttpClientService _httpClient;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientService httpClient)
        {
            m_logger = logger;
            // m_clientFactory = clientFactory;
            _httpClient = httpClient;

            m_jobPingUri = new Uri($"http://webapi/Jobs/Ping/");
            m_skillsPingUri = new Uri($"http://webapi/Skills/Ping/");
            //m_r_client = new ResilientHttpClient(m_clientFactory.CreateClient());

        }
        //public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory, IOptionsSnapshot<AppChaosSettings> chaosOptionsSnapshot, Lazy<Task<AppChaosSettings>> generalChaosSettingFactory)
        //{
        //    m_logger = logger;
        //    m_clientFactory = clientFactory;
        //    m_chaosSettings = chaosOptionsSnapshot.Value;
        //    m_generalChaosSettingFactory = generalChaosSettingFactory.Value;
        //    m_jobPingUri = new Uri($"http://webapi/Jobs/Ping/");
        //    m_skillsPingUri = new Uri($"http://webapi/Skills/Ping/");
        //    m_r_client = new ResilientHttpClient(m_clientFactory.CreateClient());

        //}

        public async Task OnGet()
        {
            
            


           

            //using (var client = m_clientFactory.CreateClient())
            {
                //    Job skTemp = new Job { m_id = 0, 
                //        m_company = "company",
                //        m_title = "title",
                //        m_start = DateTime.Now,
                //        m_end = DateTime.Now};
                //    var jobstringJson = JsonSerializer.Serialize(skTemp);
                //   /// string skillStringPlain = $"{name},{exp}";
                //    Console.WriteLine(jobstringJson);
                //    //var skillStringJson2 = JsonSerializer.Serialize(skillStringPlain);
                //    //Console.WriteLine(jobstringJson);
                //    Console.WriteLine();


                //    //if (name != null && exp != null)
                //    {
                //        var request1 = new System.Net.Http.HttpRequestMessage();
                //        var reqURI = new Uri($"http://webapi/Jobs/CreateJob/");

                //        var response = await client.PostAsJsonAsync(reqURI.ToString(), jobstringJson);
                //        //var response2 = await client.PostAsync("http://webapi/Skills/CreateSkill/", cnt);
                //        //var response = await client.PostAsJsonAsync(reqURI.ToString(), skillStringPlain);
                

                //    }
            }
            //var pingURI = new Uri($"http://webapi/Jobs/Ping/");

            //var pingResponse = await client. (pingURI.ToString(), jobstringJson);
            await Ping();
        }
        private async Task Ping()
        {
            await SkillsPing();
            await JobsPing();
            return;
        }
        private async Task<int> JobsPing()
        {
            //using(var _httpClient = m_clientFactory.CreateClient())
            //{
            //    var generalChaosSetting = await m_generalChaosSettingFactory;
            //    var context = new Context("JobsPing").WithChaosSettings(generalChaosSetting);
            //    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, m_jobPingUri);
            //    var response = await m_r_client.SendAsync(req, context);

            //}
            
            //old
            //var client = new ResilientHttpClient(m_clientFactory.CreateClient());
            //var client = m_clientFactory.CreateClient();
            var pingURI = new Uri($"http://webapi/Jobs/Ping/");
            //HttpResponse pingResponse = null;
            //Context context = new Context(nameof(JobsPing)).WithChaosSettings(chaosSettings);
            //var pingResponse = await client.GetAsyncUsingContext(pingURI.ToString(),context);
            var pingResponse = await _httpClient.GetAsync(pingURI.ToString());
            Log.Logger.Information($"Ping Jobs Controler :   {pingResponse.Content.ReadAsStringAsync().Result}");
            Log.Logger.Information($"Ping Response code :   {pingResponse.StatusCode}");
            return 0;
        }
        
        private async Task<int> SkillsPing()
        {

            //using (var _httpClient = m_clientFactory.CreateClient())
            //{
            //    var generalChaosSetting = await m_generalChaosSettingFactory;
            //    var context = new Context(nameof(SkillsPing)).WithChaosSettings(generalChaosSetting);
            //    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, m_skillsPingUri);
            //    var response = await m_r_client.SendAsync(req, context);

            //}
            {
                //var client = m_clientFactory.CreateClient();
                //var pingResponse = await client.GetAsync(m_skillsPingUri.ToString());
                var pingResponse = await _httpClient.GetAsync(m_skillsPingUri.ToString());
                Log.Logger.Information($"Ping Skills Controler  :   {pingResponse.Content.ReadAsStringAsync().Result}");
                Log.Logger.Information($"Ping Response code     :   {pingResponse.StatusCode}");
            }
            
            return 0;
        }
        class Job
        {
            public int m_id { get; set; }
            public string m_title { get; set; }
            public string m_company { get; set; }
            public DateTime m_start { get; set; }
            public DateTime? m_end { get; set; }
        }
    }
    
}
