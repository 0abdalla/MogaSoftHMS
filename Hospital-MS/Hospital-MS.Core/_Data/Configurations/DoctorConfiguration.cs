using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital_MS.Core.Enums;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {

            builder.Property(d => d.FullName)
                .IsRequired()
                .HasMaxLength(550);

            builder.Property(d => d.NationalId)
                .HasMaxLength(50);

            builder.Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(d => d.Degree)
               .HasMaxLength(55);

            builder.Property(d => d.Email)
                .HasMaxLength(100);

            builder.Property(d => d.Address)
                .HasMaxLength(500);

            builder.Property(d => d.Address)
                .HasMaxLength(1000);

            builder.Property(d => d.PhotoUrl)
                .HasMaxLength(500);

            builder.Property(d => d.DateOfBirth)
                .HasColumnType("date");

            builder.Property(d => d.StartDate)
                .HasColumnType("date");

            builder.Property(p => p.Status)
            .HasConversion(
            (type) => type.ToString(),
            (stu) => Enum.Parse<StaffStatus>(stu, true)).HasMaxLength(55);

            builder.Property(p => p.Gender)
                     .HasConversion(
                     (type) => type.ToString(),
                     (gen) => Enum.Parse<Gender>(gen, true)).HasMaxLength(55);

            builder.Property(p => p.MaritalStatus)
                     .HasConversion(
                     (type) => type.ToString(),
                     (gen) => Enum.Parse<MaritalStatus>(gen, true)).HasMaxLength(55);
        }
    }
}
