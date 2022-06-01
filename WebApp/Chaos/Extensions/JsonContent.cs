//using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApp.Chaos.Extensions
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json")
        { }
    }
}
