using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CoreTask = TaskBase.Core.TaskAggregate.Task;

namespace TaskBase.Data.EntityConfiguration
{
    public class TaskConfig : IEntityTypeConfiguration<CoreTask>
    {
        public void Configure(EntityTypeBuilder<CoreTask> builder)
        {
            builder.ToTable("Tasks")
                .HasIndex(x => x.Id);

            builder.HasMany(x => x.Notes)
                .WithOne()
                .HasForeignKey("taskId");
        }
    }
}
