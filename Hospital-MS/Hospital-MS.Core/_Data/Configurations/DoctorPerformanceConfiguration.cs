using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class DoctorPerformanceConfiguration : IEntityTypeConfiguration<DoctorRating>
    {
        public void Configure(EntityTypeBuilder<DoctorRating> builder)
        {
            //builder.Property(dp => dp.EvaluationDate)
            //   .IsRequired();

            builder.Property(dp => dp.Rating)
                   .HasColumnType("decimal(12,2)")
                   .IsRequired();

            builder.Property(dp => dp.Comments)
                   .HasMaxLength(500) 
                   .IsRequired(false); 
        }
    }
}
