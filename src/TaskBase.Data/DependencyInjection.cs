using Microsoft.Extensions.DependencyInjection;
using TaskBase.Data.Repositories;
using TaskBase.Core.Interfaces;
using TaskBase.Data.Utils;
using Microsoft.Extensions.Configuration;

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
                return new ConnectionString("TaskDb", Configuration.GetConnectionString("TaskDb"));
            });

            services.AddDbContext<TaskDbContext>();
            services.AddTransient<ITaskAsyncRepository, InMemoryTaskRepository>();

            return services;
        }
    }
}
