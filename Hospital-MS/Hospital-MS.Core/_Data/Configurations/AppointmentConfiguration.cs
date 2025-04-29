using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {         
            builder.Property(b => b.Status)
                .HasConversion(
                (type) => type.ToString(),
                (gen) => Enum.Parse<AppointmentStatus>(gen, true)).HasMaxLength(55);

            builder.Property(b => b.Type)
               .HasConversion(
               (type) => type.ToString(),
               (gen) => Enum.Parse<AppointmentType>(gen, true)).HasMaxLength(55);
        }
    }
}
