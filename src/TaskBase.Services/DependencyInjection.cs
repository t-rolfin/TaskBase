using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Services.Repositories;

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
        public static IServiceCollection AddInfrastructure(this ServiceCollection services)
        {

            services.AddTransient<ITaskAsyncRepository, InMemoryTaskRepository>();

            return services;
        }
    }
}
