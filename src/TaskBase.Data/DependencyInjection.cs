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
using TaskBase.Data.NotificationService;
using TaskBase.Application.Services;
using TaskBase.Data.Storage;

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

            services.AddDbContext<TaskContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITaskAsyncRepository, TaskRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<IQueryRepository, QueryRepository>();

            services.AddTransient<INotificationService, NotificationService.NotificationService>();
            services.AddTransient<IImageStorage, ImageStorage>();
            services.AddTransient<IAuthTokenFactory, AuthTokenFactory>();
            services.AddTransient<ILoginService<User>, LoginService>();

            return services;
        }

        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
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

            return services;
        }

        public static IServiceCollection AddCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                });

            services.Configure<SecurityStampValidatorOptions>(options => {
                options.ValidationInterval = TimeSpan.Zero;
            });

            return services;
        }
    }
}
