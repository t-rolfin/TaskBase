using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskUser = TaskBase.Core.TaskAggregate.User;

namespace TaskBase.Data.EntityConfiguration
{
    public class UserConfig : IEntityTypeConfiguration<TaskUser>
    {
        public void Configure(EntityTypeBuilder<TaskUser> builder)
        {
            builder.ToTable("users")
                .HasKey(x => x.Id);
        }
    }
}
