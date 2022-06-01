using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CV;

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
            var result = await Methods.GetAllSkills(m_clientFactory);
            return result;
            

        }
    }
}
