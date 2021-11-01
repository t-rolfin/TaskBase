using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Shared;
using TaskBase.Core.TaskAggregate;
using TaskBase.Data.Identity;

namespace TaskBase.Data.Utils
{
    public class TaskContextSeed
    {
        public async System.Threading.Tasks.Task SeedAsync(
            IdentityContext identityContext, TaskContext context)
        {
            await SeedTaskUsersAsync(identityContext, context);
            await SeedPriorityLevelAsync(context);
        }

        async System.Threading.Tasks.Task SeedTaskUsersAsync(
            IdentityContext identityContext, TaskContext context)
        {
            if(!context.Set<Core.TaskAggregate.User>().Any())
            {
                var user = await identityContext.Users.Where(x => x.UserName.Equals("Admin"))
                    .FirstOrDefaultAsync();

                var taskUser = new Core.TaskAggregate.User(
                    Guid.Parse(user.Id), user.UserName);

                await context.Set<Core.TaskAggregate.User>().AddAsync(taskUser);
                await context.SaveChangesAsync();
            }
        }

        async System.Threading.Tasks.Task SeedPriorityLevelAsync(TaskContext context)
        {
            if (!context.Set<PriorityLevel>().Any())
            {
                foreach (var priorityLevel in Enumeration.GetAll<PriorityLevel>())
                {
                    await context.Set<PriorityLevel>().AddAsync(
                        new PriorityLevel(priorityLevel.Value, priorityLevel.DisplayName)
                        );
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
