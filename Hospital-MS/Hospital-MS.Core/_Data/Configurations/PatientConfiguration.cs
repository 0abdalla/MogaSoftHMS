using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(250);


            builder.Property(p => p.DateOfBirth)
                   .HasColumnType("date");

            builder.Property(p => p.Gender)
                     .HasConversion(
                     (type) => type.ToString(),
                     (gen) => Enum.Parse<Gender>(gen, true)).HasMaxLength(55);

            builder.Property(p => p.Status)
                     .HasConversion(
                     (type) => type.ToString(),
                     (stu) => Enum.Parse<PatientStatus>(stu, true)).HasMaxLength(55);

            builder.Property(p => p.Phone)
                   .HasMaxLength(20);


            builder.Property(p => p.Email)
                   .HasMaxLength(100);

            builder.Property(p => p.Notes)
                   .HasMaxLength(1000);


            builder.Property(p => p.Address)
                   .HasMaxLength(255);

            builder.Property(p => p.EmergencyPhone01)
                   .HasMaxLength(20);

            builder.Property(p => p.EmergencyContact01)
                   .HasMaxLength(250);
            
            builder.Property(p => p.EmergencyPhone02)
                   .HasMaxLength(20);

            builder.Property(p => p.EmergencyContact02)
                   .HasMaxLength(250);
        }
    }
}
