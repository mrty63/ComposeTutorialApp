using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CV;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class CVModel : PageModel
    {
        private readonly IHttpClientFactory m_clientFactory;

        public CVModel(IHttpClientFactory clientFactory)
        {
            m_clientFactory = clientFactory;
        }

        public void OnGet()
        {
        }
        public async Task<List<Education>> GetEducation()
        {
            var result = await Methods.GetEducation(m_clientFactory);
            return result;
        }
        public async Task<List<Skill>> GetSkills()
        {
            var result = await Methods.GetAllSkills(m_clientFactory);
            return result;
        }
    }
}
