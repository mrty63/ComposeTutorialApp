using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class ListAllSkillsModel : PageModel
    {
        private readonly IHttpClientFactory m_clientFactory;


        public ListAllSkillsModel(IHttpClientFactory clientFactory)
        {
            m_clientFactory = clientFactory;
        }
        public void OnGet()
        {
            
        }
        public async Task<List<Skill>>GetSkills()
        {
            string strResult;
            using (var client = m_clientFactory.CreateClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                string uriString = "http://webapi/Skills/GetAllSkills";
                request.RequestUri = new Uri(uriString);
                //var response1 = await client.SendAsync(request);
                var response2 = await client.GetAsync(uriString);
                strResult = await response2.Content.ReadAsStringAsync();


            }

            List<Skill> lRes = JsonSerializer.Deserialize<List<Skill>>(strResult);
            return lRes;

        }
    }
}
