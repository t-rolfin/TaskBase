using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskBase.Data;
using TaskBase.Core.Interfaces;
using TaskBase.Core.Facades;
using System.Globalization;
using TaskBase.Components.Services;
using TaskBase.Data.Storage;
using TaskBase.RazorPages.Services;

namespace TaskBase.RazorPages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddRazorOptions(options => { options.PageViewLocationFormats.Add("/Views/{0}.cshtml"); })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddRazorPages();
            services.AddIdentity();

            services.AddAuthorization(options => {
                options.AddPolicy("Admin", x => x.RequireRole("Admin"));
            });

            services.AddPortableObjectLocalization(opt => { opt.ResourcesPath = "Resources"; });

            services.AddInfrastructure(Configuration);
            services.AddTransient<IFacade, Facade>();
            services.AddTransient<IIdentityProvider, IdentityProvider>();
            services.AddTransient<IImageStorage, ImageStorage>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRequestLocalization(GetLocalizationOptions());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }

        private RequestLocalizationOptions GetLocalizationOptions()
        {
            var supportedCultures = Configuration.GetSection("Cultures").GetChildren().ToList()
            .Select(x => { return new CultureInfo(x.Key); }).ToList();

            var options = new RequestLocalizationOptions();
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            return options;
        }
    }
}
