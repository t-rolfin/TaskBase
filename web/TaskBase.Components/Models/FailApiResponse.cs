using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Components.Models
{
    public class FailApiResponse
    {
        //{
        //    "errors": [
        //        "'Title' must not be empty."
        //    ],
        //    "type": null,
        //    "title": null,
        //    "status": 400,
        //    "detail": "Please refer to the errors property for additional details.",
        //    "instance": "/api/Task"
        //}

        public FailApiResponse()
        {
            Errors = new();
        }

        public FailApiResponse(List<string> errors, string type, int status, string instance)
        {
            Errors = errors;
            Type = type;
            Status = status;
            Instance = instance;
        }

        public List<string> Errors { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public string Instance { get; set; }
    }
}
