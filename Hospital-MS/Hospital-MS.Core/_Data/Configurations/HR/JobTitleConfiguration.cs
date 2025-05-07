using Hospital_MS.Core.Models.HR;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core._Data.Configurations.HR
{
    class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
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
