using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskBase.Components.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TaskBase.Components.Views.Components.Culture
{
    public class CultureViewComponent : ViewComponent
    {
        private readonly IConfiguration _configs;

        public CultureViewComponent(IConfiguration configs)
        {
            _configs = configs;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var cultureFeature = System.Threading.Thread.CurrentThread.CurrentUICulture;

                var model = new CultureSwitchModel()
                {
                    CurrentUICulture = cultureFeature,
                    SupportedCultures = _configs.GetSection("Cultures").GetChildren()
                    .Select(x => { return new System.Globalization.CultureInfo(x.Key); }).ToList()
                };

                return View(model);
            });
            
        }
    }
}
