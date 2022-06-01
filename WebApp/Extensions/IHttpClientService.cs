using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApp.Extensions
{
    public interface IHttpClientService
    {
       
        Task<HttpResponseMessage>GetAsync(string uri);
    }
}
