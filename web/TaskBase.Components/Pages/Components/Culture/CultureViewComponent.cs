using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaskBase.Components.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TaskBase.Components.Pages.Components.Culture
{
    public class CultureViewComponent : ViewComponent
    {
        private readonly IOptions<RequestLocalizationOptions> localizationOptions;

        public CultureViewComponent(IOptions<RequestLocalizationOptions> localizationOptions)
            => this.localizationOptions = localizationOptions;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();

                var model = new CultureSwitchModel()
                {
                    CurrentUICulture = cultureFeature.RequestCulture.UICulture,
                    SupportedCultures = localizationOptions.Value.SupportedUICultures.ToList()
                };

                return View(model);
            });
            
        }
    }
}
