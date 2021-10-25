using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBase.Application.Utils
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
