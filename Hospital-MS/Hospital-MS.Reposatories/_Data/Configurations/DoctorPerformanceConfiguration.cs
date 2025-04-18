using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class DoctorPerformanceConfiguration : IEntityTypeConfiguration<DoctorPerformance>
    {
        public void Configure(EntityTypeBuilder<DoctorPerformance> builder)
        {
            builder.Property(dp => dp.EvaluationDate)
               .IsRequired();

            builder.Property(dp => dp.Rating)
                   .HasColumnType("decimal(12,2)")
                   .IsRequired();

            builder.Property(dp => dp.Comments)
                   .HasMaxLength(500) 
                   .IsRequired(false); 
        }
    }
}
