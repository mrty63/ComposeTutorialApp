using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
//using System.Threading.Tasks;
using CV;

namespace WebApp.Pages
{
    public class AddItemModel : PageModel
    {
        private readonly IHttpClientFactory m_clientFactory;
        private Skill sk;
        [BindProperty]
        public string m_jobTitle { get; set; }
        [BindProperty]
        public string m_jobCompany { get; set; }
        [BindProperty]
        public string m_jobStart { get; set; }
        [BindProperty]
        public string? m_jobEnd { get; set; }
        [BindProperty]
        public string? m_skName { get; set; }
        [BindProperty]
        public string? m_skExp { get; set; }
        [BindProperty]
        public int m_choice { get; set; }
        [BindProperty]
        public string m_edName { get; set; }
        [BindProperty]
        public string m_edSchool { get; set; }
        [BindProperty]
        public string m_edStart { get; set; }
        [BindProperty]
        public string m_edEnd { get; set; }
        [BindProperty]
        public string m_edGrade { get; set; }
        public string resultOfPost { get; set; }
        public DateTime resultOfDatePost { get; set; }

        public AddItemModel(IHttpClientFactory clientFactory)
        {
            m_clientFactory = clientFactory;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            //var title = Request.Form["m_title"];
            //var companyName = Request.Form["m_company"];
            //var start= Request.Form["m_start"];
            //var end = Request.Form["m_end"];
            //DateTime resultOfDatePost = DateTime.Parse(m_start);
            //resultOfPost = $"{m_title} {m_company} {m_start}";

            bool postSuccess = false;
            switch (m_choice)
            {
                case 1: postSuccess = await PostJob();
                    break;
                case 2: postSuccess = await PostSkill();
                    break;
                case 3: postSuccess = await PostEducation();
                    break;
                default:
                    break;
            }
            if(postSuccess)
            {
                return RedirectToPage("./AddItem");
            }
            else
            {
                throw new NotImplementedException("insertion-fail");

            }

        }

        public async Task<bool> PostJob()
        {
            using (var client = m_clientFactory.CreateClient())
            {
                Job job;
                job = new Job()
                {
                    m_title = this.m_jobTitle,
                    m_company = this.m_jobCompany,
                    m_start = DateTime.Parse(this.m_jobStart),
                };
                if (string.IsNullOrEmpty(this.m_jobEnd))
                {

                    job.m_end = null;

                }
                else
                {
                    job.m_end = DateTime.Parse(this.m_jobEnd);

                }
                job.m_id = 1;

                var jobstringJson = JsonSerializer.Serialize(job);
                /// string skillStringPlain = $"{name},{exp}";
                //Console.WriteLine(jobstringJson);
                //var skillStringJson2 = JsonSerializer.Serialize(skillStringPlain);
                //Console.WriteLine(jobstringJson);
                //Console.WriteLine();


                //if (name != null && exp != null)

                //var request1 = new System.Net.Http.HttpRequestMessage();
                var reqURI = new Uri($"http://webapi/Jobs/CreateJob/");

                var response = await client.PostAsJsonAsync(reqURI.ToString(), jobstringJson);
                //var response2 = await client.PostAsync("http://webapi/Skills/CreateSkill/", cnt);
                //var response = await client.PostAsJsonAsync(reqURI.ToString(), skillStringPlain);


                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }

        }

        public async Task<bool> PostSkill()
        {

            Skill skill = new Skill() { m_name = m_skName, m_exp = m_skExp };

            var skillStringJson = JsonSerializer.Serialize(skill);
            var client = m_clientFactory.CreateClient();
            var reqURI = new Uri($"http://webapi/Skills/CreateSkill/");
            var response = await client.PostAsJsonAsync(reqURI.ToString(), skillStringJson);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;

            }
        }

        public async Task<bool> PostEducation()
        {
            Education edu = new Education()
            {
                m_title = m_edName,
                m_school = m_edSchool,
                m_grade = m_edGrade,
                m_start = DateTime.Parse(m_edStart),
                m_end = DateTime.Parse(m_edEnd)
            };

            var eduStringJson = JsonSerializer.Serialize(edu);
            var client = m_clientFactory.CreateClient();
            var reqURI = new Uri($"http://webapi/Education/CreateEducation/");
            var response = await client.PostAsJsonAsync(reqURI.ToString(), eduStringJson);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;

            }


            //throw new NotImplementedException("cannot Post educatioon yet");

            //return false;
        }
    }
    //public class Job
    //{
    //    public int m_id { get; set; }
    //    public string m_title { get; set; }
    //    public string m_company { get; set; }
    //    public DateTime m_start { get; set; }
    //    public DateTime? m_end { get; set; }
    //}
}