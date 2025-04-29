using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class AdmissionConfiguration : IEntityTypeConfiguration<Admission>
    {
        public void Configure(EntityTypeBuilder<Admission> builder)
        {
            builder.Property(a => a.Notes)
                .HasMaxLength(750);

            builder.Property(a => a.HealthStatus)
            .HasMaxLength(200);

            builder.Property(a => a.InitialDiagnosis)
                .HasMaxLength(500);

            builder.Property(a => a.CompanionName)
                .HasMaxLength(100);

            builder.Property(a => a.CompanionNationalId)
                .HasMaxLength(20);

            builder.Property(a => a.CompanionPhone)
                .HasMaxLength(20);
        }
    }
}
