using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Utils
{
    public static class HttpResponseParser
    {
        public static async Task<T> Parse<T>(HttpContent content)
        {
            string Content = await content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(Content);

            return deserializedObject;
        }
    }
}
