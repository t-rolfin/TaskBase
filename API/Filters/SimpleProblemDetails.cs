using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Filters
{
    public class SimpleProblemDetails : ProblemDetails
    {
        public SimpleProblemDetails() : base()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
    }
}
