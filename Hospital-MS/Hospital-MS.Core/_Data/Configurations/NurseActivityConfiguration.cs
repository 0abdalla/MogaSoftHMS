using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class NurseActivityConfiguration : IEntityTypeConfiguration<NurseActivity>
    {
        public void Configure(EntityTypeBuilder<NurseActivity> builder)
        {
            builder.Property(na => na.ActivityDate)
               .IsRequired();

            builder.Property(na => na.ActivityDescription)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
