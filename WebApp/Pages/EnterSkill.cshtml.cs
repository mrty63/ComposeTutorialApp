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

        public async Task<IActionResult> OnPostAsync(string name,string exp)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            using (var client = m_clientFactory.CreateClient())
            {
                Console.WriteLine($"input name  : {name}");
                Console.WriteLine($"input exp   : {exp}");
                Skill skTemp = new Skill { m_name = name, m_exp = exp };
                var skillStringJson = JsonSerializer.Serialize(skTemp);
                string skillStringPlain = $"{name},{exp}";
                Console.WriteLine(skillStringJson);
                var skillStringJson2 = JsonSerializer.Serialize(skillStringPlain);
                Console.WriteLine(skillStringJson2);
                Console.WriteLine();
                if (name != null && exp != null)
                {
                    var request1 = new System.Net.Http.HttpRequestMessage();
                    //request1.RequestUri = new Uri($"http://webapi/Skills/CreateSkill/");
                    var reqURI = new Uri($"http://webapi/Skills/CreateSkill/");
                    //request1.Content = new StringContent(skillString);
                    var reqContent = new StringContent(skillStringPlain);
                    var reqContent2 = (HttpContent)reqContent;
                    var reqContent3 = new StringContent(skillStringJson, Encoding.UTF8, "application/json");
                    var reqContent4 = new StringContent(skillStringPlain, Encoding.UTF8, "text/plain");
                    var reqContent5 = new StringContent(skillStringPlain, Encoding.ASCII, "text/plain");
                    request1.Content = reqContent3;
                    request1.Method = HttpMethod.Post;
                    var response = await client.PostAsJsonAsync(reqURI.ToString(), skillStringPlain);
                    //var response3 = await client.SendAsync(request1);
                    //var a_name  = await client.PostAsync(reqURI, reqContent3);
                }


            }
                

            return RedirectToPage("./Index");
        }
    }
    public class Skill
    {
        public string m_name { get; set; }
        public string m_exp { get; set; }
    }   
}

