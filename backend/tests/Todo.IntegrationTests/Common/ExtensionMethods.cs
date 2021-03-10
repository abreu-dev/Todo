using Todo.Application.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Todo.IntegrationTests.Common
{
    public static class ExtensionMethods
    {
        public static StringContent ToStringContent(this DTO dto)
        {
            return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        }

        public static async Task<TObject> ToObject<TObject>(this HttpContent httpContent)
        {
            var json = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TObject>(json);
        }
    }

    public class ValidationResult
    {
        public ICollection<string> Errors { get; set; }
    }

    public class MiddlewareResult
    {
        public string Error { get; set; }
    }
}
