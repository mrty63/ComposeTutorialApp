using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CV;
using Serilog;

namespace WebApp.Pages
{
    public class EnterSkillModel : PageModel
    {
        private readonly IHttpClientFactory m_clientFactory;

        public EnterSkillModel(IHttpClientFactory clientFactory)
        {
            
            m_clientFactory = clientFactory;
        }
        public void OnGet()
        {
            
        }

        //public async Task<IActionResult> OnPostAsync(string name,string exp)
        public IActionResult OnPostAsync(string name,string exp)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                EnterSkill(name, exp);

            }
            catch (Exception e)
            {
                Log.Logger.Error($"exception: {e.GetType()}");
                //throw e;
            }



            return RedirectToPage("./Index");
        }

        private async void EnterSkill(string name, string exp)
        {
            
            using (var client = m_clientFactory.CreateClient())
            {
                Console.WriteLine($"input name  : {name}");
                Console.WriteLine($"input exp   : {exp}");
                Skill skTemp = new() { m_name = name, m_exp = exp };
                var skillStringJson = JsonSerializer.Serialize(skTemp);
                string skillStringPlain = $"{name},{exp}";
                Console.WriteLine(skillStringJson);
                var skillStringJson2 = JsonSerializer.Serialize(skillStringPlain);
                Console.WriteLine(skillStringJson2);
                Console.WriteLine();


                if (name != null && exp != null)
                {
                    var request1 = new System.Net.Http.HttpRequestMessage();
                    var reqURI = new Uri($"http://webapi/Skills/CreateSkill/");

                    var response = await client.PostAsJsonAsync(reqURI.ToString(), skillStringJson);
                    if(response.IsSuccessStatusCode)
                    {
                         Log.Logger.Debug($"skill    {name} successfully created in redis");
                    }
                }


            }
        }
    }
       
}

