using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskBase.Core.Facades;
using TaskBase.Core.Interfaces;
using TaskBase.Services;

namespace TaskBase.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorOptions(options => { options.ViewLocationFormats.Add("/Views/{0}.cshtml"); })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddPortableObjectLocalization(x => x.ResourcesPath = "Resources");

            services.Configure((Action<RequestLocalizationOptions>)(x =>
            {
                GetCultures(x);
            }));

            services.AddInfrastructure();
            services.AddTransient<ITaskFacade, TaskFacade>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRequestLocalization();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void GetCultures(RequestLocalizationOptions x)
        {
            var cultures = Configuration.GetSection("Cultures")
                            .GetChildren()
                            .ToDictionary(x => x.Key, y => y.Value)
                            .Keys
                            .ToArray();

            x.AddSupportedCultures(cultures);
            x.AddSupportedUICultures(cultures);
        }
    }
}