

using CV;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApp
{
    static public class Methods
    {
        
        public static async Task<List<Skill>> GetAllSkills(IHttpClientFactory clientFactory)
        {
            string strResult = null;
            using (var client = clientFactory.CreateClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                string uriString = "http://webapi/Skills/GetAllSkills";
                request.RequestUri = new Uri(uriString);
                //var response1 = await client.SendAsync(request);
                HttpResponseMessage response2 = null;
                response2 = await client.GetAsync(uriString);

                strResult = await response2.Content.ReadAsStringAsync();




            }

            List<Skill> lRes = JsonSerializer.Deserialize<List<Skill>>(strResult);
            if (strResult == null || lRes.Count == 0)
            {
                return null;
            }
            else
            {

                return lRes;
            }


        }
        public static async Task<List<Education>> GetEducation(IHttpClientFactory clientFactory)
        {
            string strResult = null;
            using (var client = clientFactory.CreateClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                string uriString = "http://webapi/Education/GetAllEducation";
                request.RequestUri = new Uri(uriString);
                //var response1 = await client.SendAsync(request);
                HttpResponseMessage response2 = null;
                response2 = await client.GetAsync(uriString);
                if (!response2.IsSuccessStatusCode)
                {
                    return null;
                }

                strResult = await response2.Content.ReadAsStringAsync();



            }

            List<Education> listRes = JsonSerializer.Deserialize<List<Education>>(strResult);

            return listRes;



        }
        public static async Task<List<Job>> GetJobs(IHttpClientFactory clientFactory)
        {
            string strResult = null;
            using (var client = clientFactory.CreateClient())
            {
                //var request = new System.Net.Http.HttpRequestMessage();
                string uriString = "http://webapi/Jobs/GetAllJobs";
                //request.RequestUri = new Uri(uriString);
                //var response1 = await client.SendAsync(request);
                HttpResponseMessage response2 = null;
                response2 = await client.GetAsync(uriString);
                if (!response2.IsSuccessStatusCode)
                {
                    return null;
                }

                strResult = await response2.Content.ReadAsStringAsync();



            }

            List<Job> listRes = JsonSerializer.Deserialize<List<Job>>(strResult);

            return listRes;



        }
    }
    

}



