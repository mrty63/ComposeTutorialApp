using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    public class AddItemModel : PageModel
    {
        private readonly IHttpClientFactory m_clientFactory;

        [BindProperty]
        public string m_title { get; set; }
        [BindProperty]
        public string m_company { get; set; }
        [BindProperty]
        public string m_start { get; set; }
        public string? m_end { get; set; }

        public string resultOfPost { get; set; }
        public DateTime resultOfDatePost { get; set; }

        public AddItemModel(IHttpClientFactory clientFactory)
        {
            m_clientFactory = clientFactory;
        }
        public void OnGet()
        {
        }

        public async void OnPost()
        {
            //var title = Request.Form["m_title"];
            //var companyName = Request.Form["m_company"];
            //var start= Request.Form["m_start"];
            //var end = Request.Form["m_end"];
            DateTime resultOfDatePost = DateTime.Parse(m_start);
            resultOfPost = $"{m_title} {m_company} {m_start}";

            using (var client = m_clientFactory.CreateClient())
            {
                Job job;
                job = new Job()
                {
                    m_title = this.m_title,
                    m_company = this.m_company,
                    m_start = DateTime.Parse(this.m_start),
                };
                if (string.IsNullOrEmpty(m_end))
                {

                    job.m_end = null;
                    
                }
                else
                {
                        job.m_end = DateTime.Parse(this.m_end);
                   
                }
                job.m_id = 1;

                var jobstringJson = JsonSerializer.Serialize(job);
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

    }
    public class Job
    {
        public int m_id { get; set; }
        public string m_title { get; set; }
        public string m_company { get; set; }
        public DateTime m_start { get; set; }
        public DateTime? m_end { get; set; }
    }
}
