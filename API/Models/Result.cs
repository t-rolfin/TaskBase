using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Result<T>
        where T : class
    {
        public Result(T data, List<Dictionary<string, string>> links)
        {
            Data = data;
            Links = links;
        }

        public T Data { get; set; }
        public List<Dictionary<string, string>> Links { get; set; }
    }
}
