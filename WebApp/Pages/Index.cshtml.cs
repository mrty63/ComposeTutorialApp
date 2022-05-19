using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;


namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> m_logger;
        private readonly IHttpClientFactory m_clientFactory;


        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
        {
            m_logger = logger;
            m_clientFactory = clientFactory;
        }

        public async Task OnGet()
        {
            //string redisQuery = "hello";

            ////ViewData["Message"] = "Hello from  default webfrontend";
            ////ViewData["Message1Body"] = "weather JSON here";
            //ViewData["Message2"] = "Key: value";
            //ViewData["Message2Body"] = $"{redisQuery} : ";


            //using (var client = new System.Net.Http.HttpClient())
            //using (var client =m_clientFactory.CreateClient())
            //{
            //    //// Call *webapi*, and display its response in the page
            //    //var request = new System.Net.Http.HttpRequestMessage();
            //    //request.RequestUri = new Uri("http://webapi/WeatherForecast");
            //    ////request.RequestUri = new Uri("http://localhost:6880/WeatherForecast");    //non docker api call
            //    //var response = await client.SendAsync(request);
            //    //ViewData["Message1Body"] = await response.Content.ReadAsStringAsync();

            //    var request1 = new System.Net.Http.HttpRequestMessage();
            //    request1.RequestUri = new Uri($"http://webapi/Skills/");
            //    var response3 = await client.SendAsync(request1);
            //    var result = Convert.ToBoolean( await  response3.Content.ReadAsStringAsync() );
            //    if( result )
            //    {
            //        ViewData["Message"] = "Hello already present in redis";
            //    }
            //    else
            //    {
            //        ViewData["Message"] = "Hello created in redis";

            //    }

            //    var request2 = new System.Net.Http.HttpRequestMessage();
            //    request2.RequestUri = new Uri($"http://webapi/Skills/{redisQuery}");
            //    var response2 = await client.SendAsync(request2);
            //    ViewData["Message2Body"] += await response2.Content.ReadAsStringAsync();




            //    //request.RequestUri = new Uri("http://webapi/Skills/");
            //}

            using (var client = m_clientFactory.CreateClient())
            {
                Job skTemp = new Job { m_id = 0, 
                    m_company = "company",
                    m_title = "title",
                    m_start = DateTime.Now,
                    m_end = DateTime.Now};
                var jobstringJson = JsonSerializer.Serialize(skTemp);
               /// string skillStringPlain = $"{name},{exp}";
                Console.WriteLine(jobstringJson);
                //var skillStringJson2 = JsonSerializer.Serialize(skillStringPlain);
                //Console.WriteLine(jobstringJson);
                Console.WriteLine();

                
                //if (name != null && exp != null)
                {
                    var request1 = new System.Net.Http.HttpRequestMessage();
                    var reqURI = new Uri($"http://webapi/Jobs/CreateJob/");

                    var response = await client.PostAsJsonAsync(reqURI.ToString(), jobstringJson);
                    //var response2 = await client.PostAsync("http://webapi/Skills/CreateSkill/", cnt);
                    //var response = await client.PostAsJsonAsync(reqURI.ToString(), skillStringPlain);

                }
            }
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
