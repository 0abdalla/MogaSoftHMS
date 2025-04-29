using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class NurseShiftConfiguration : IEntityTypeConfiguration<NurseShift>
    {
        public void Configure(EntityTypeBuilder<NurseShift> builder)
        {
            builder.Property(ns => ns.ShiftDate)
              .IsRequired();

            builder.Property(ns => ns.StartTime)
                   .IsRequired();

            builder.Property(ns => ns.EndTime)
                   .IsRequired();
        }
    }
}
