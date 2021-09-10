using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace TaskBase.RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStringLocalizer<IndexModel> stringLocalizer;
        public IndexModel(ILogger<IndexModel> logger, IStringLocalizer<IndexModel> stringLocalizer)
        {
            _logger = logger;
            this.stringLocalizer = stringLocalizer;
        }

        public string WelcomeMessage { get { return stringLocalizer["Welcome"]; } }

        public void OnGet()
        {
            ViewData["Home"] = stringLocalizer["Home"];
        }
    }
}
