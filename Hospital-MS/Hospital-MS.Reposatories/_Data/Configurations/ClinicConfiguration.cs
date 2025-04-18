using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Type)
                     .HasConversion(
                     (type) => type.ToString(),
                     (stu) => Enum.Parse<ClinicType>(stu, true)).HasMaxLength(55);

        }
    }   
}
