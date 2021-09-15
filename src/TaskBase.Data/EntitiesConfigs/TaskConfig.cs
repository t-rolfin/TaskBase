using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Data.EntitiesConfigs
{
    public class TaskConfig : IEntityTypeConfiguration<CoreTask>
    {
        public void Configure(EntityTypeBuilder<CoreTask> builder)
        {
            builder.ToTable("Tasks")
                .HasIndex(x => x.Id);
        }
    }
}
