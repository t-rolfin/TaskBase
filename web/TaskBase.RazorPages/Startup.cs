using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using TaskBase.Components.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using TaskBase.Components.Utils;

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
            services.AddRazorPages().AddRazorOptions(options => {
                options.PageViewLocationFormats.Add("/Views/{0}.cshtml");
            })
                .AddRazorRuntimeCompilation()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AttachTokenDelegateHandler>();
            services.AddServices(Configuration);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.Cookie.Name = "auth.cookie";
                });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
            });

            services.AddPortableObjectLocalization(opt => { opt.ResourcesPath = "Resources"; });
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
            app.UseSession();

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

    public static class DIConteinerExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration Configuration)
        {

            services.AddHttpClient<ITaskService, TaskService>(options => {
                options.BaseAddress = new Uri(Configuration["Api:BaseAddress"]);
            })
                .AddHttpMessageHandler<AttachTokenDelegateHandler>();

            services.AddHttpClient<IAuthService, AuthService>(options => {
                options.BaseAddress = new Uri(Configuration["Api:BaseAddress"]);
            })
                .AddHttpMessageHandler<AttachTokenDelegateHandler>();

            services.AddHttpClient<INoteService, NoteService>(options => {
                options.BaseAddress = new Uri(Configuration["Api:BaseAddress"]);
            })
                .AddHttpMessageHandler<AttachTokenDelegateHandler>();

            services.AddHttpClient<INotificationService, NotificationService>(options => {
                options.BaseAddress = new Uri(Configuration["Api:BaseAddress"]);
            })
                .AddHttpMessageHandler<AttachTokenDelegateHandler>();

            return services;
        }
    }
}
