using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.Property(s => s.FullName)
               .IsRequired()
               .HasMaxLength(350);

            builder.Property(s => s.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(s => s.Specialization)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(s => s.PhoneNumber)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(s => s.HireDate)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(s=>s.Status)
                .HasMaxLength(100);

            builder.Property(p => p.Status)
                     .HasConversion(
                     (type) => type.ToString(),
                     (stu) => Enum.Parse<StaffStatus>(stu, true)).HasMaxLength(55);

            builder.Property(p => p.Type)
                     .HasConversion(
                     (type) => type.ToString(),
                     (stu) => Enum.Parse<StaffType>(stu, true)).HasMaxLength(55);
        }
    }
}
