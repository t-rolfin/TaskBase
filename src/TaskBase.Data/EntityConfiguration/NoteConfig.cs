using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBase.Core.TaskAggregate;

namespace TaskBase.Data.EntityConfiguration
{
    public class NoteConfig : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Notes")
                .HasKey(x => x.Id);

            builder.Ignore(x => x.EntityStatus);
            builder.Ignore(x => x.IsModified);
        }
    }
}
