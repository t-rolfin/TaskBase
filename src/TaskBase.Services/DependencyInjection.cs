using Microsoft.Extensions.DependencyInjection;
using TaskBase.Services.Repositories;
using TaskBase.Core.Interfaces;

namespace TaskBase.Services
{
    public static class DependencyInjection
    {
        /// <summary>
        /// An extension method for ServiceCollection that helps to
        /// registrate the infrastructure services.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Collection of services.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<TaskDbContext>();
            services.AddTransient<ITaskAsyncRepository, InMemoryTaskRepository>();

            return services;
        }
    }
}
