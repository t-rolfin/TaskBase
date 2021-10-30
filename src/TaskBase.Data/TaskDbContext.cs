using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.Interfaces;
using TaskBase.Core.NotificationAggregate;
using TaskBase.Data.Utils;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Data
{
    public class TaskContext : DbContext
    {
        readonly ConnectionStrings _connectionString;

        public TaskContext(ConnectionStrings connectionString) : base()
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString.GetConnectionString("TaskDb"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
