using Microsoft.Extensions.DependencyInjection;
using TaskBase.Data.Repositories;
using TaskBase.Core.Interfaces;
using TaskBase.Data.Utils;
using Microsoft.Extensions.Configuration;
using TaskBase.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.AspNetCore.Http;

namespace TaskBase.Data
{
    public static class DependencyInjection
    {
        /// <summary>
        /// An extension method for ServiceCollection that helps to
        /// registrate the infrastructure services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Collection of services.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton(x =>
            {
                var connectionStrings = new ConnectionStrings();

                connectionStrings
                    .Add("TaskDb", Configuration.GetConnectionString("TaskDb"))
                    .Add("IdentityDb", Configuration.GetConnectionString("IdentityDb"));

                return connectionStrings;
            });

            services.AddDbContext<TaskDbContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITaskAsyncRepository, TaskRepository>();

            return services;
        }


        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequiredLength = 4,
                };
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789. _";
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<SecurityStampValidatorOptions>(options => {
                options.ValidationInterval = TimeSpan.Zero;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;

                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "auth_cookie"
                };
            });

            return services;
        }
    }
}
