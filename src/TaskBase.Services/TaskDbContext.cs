using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Services
{
    public class TaskDbContext : DbContext
    {
        public DbSet<CoreTask> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TaskDb");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
