using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Utils
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<T> MapTo<T>(this HttpResponseMessage httpResponse)
        {
            string contentAsString = await httpResponse.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(contentAsString);

            return deserializedObject;
        }
    }
}
