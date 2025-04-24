using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class PatientMedicalHistoryConfiguration : IEntityTypeConfiguration<PatientMedicalHistory>
    {
        public void Configure(EntityTypeBuilder<PatientMedicalHistory> builder)
        {
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1055);
        }
    }
}
