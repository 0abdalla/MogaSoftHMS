using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations.HR
{
    public class JobLevelConfiguration : IEntityTypeConfiguration<JobLevel>
    {
        public void Configure(EntityTypeBuilder<JobLevel> builder)
        {
            builder.Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(j => j.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Status)
                     .HasConversion(
                     (type) => type.ToString(),
                     (stu) => Enum.Parse<StatusTypes>(stu, true)).HasMaxLength(55);
        }
    }
}
