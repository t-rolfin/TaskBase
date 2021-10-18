using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;
using Aggregate = TaskBase.Core.TaskAggregate;

namespace TaskBase.Data.EntityConfiguration
{
    public class PriorityLevelConfig : IEntityTypeConfiguration<Aggregate.PriorityLevel>
    {
        public void Configure(EntityTypeBuilder<Aggregate.PriorityLevel> builder)
        {
            builder.ToTable("PriorityLevels")
                .HasKey(x => x.Value);

            builder.Property(x => x.Value)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.DisplayName)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
