using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.Property(ds => ds.WeekDay)
               .IsRequired();

            builder.Property(ds => ds.StartTime)
                   .IsRequired();

            builder.Property(ds => ds.EndTime)
                   .IsRequired();
        }
    }
}
