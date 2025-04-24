using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.NationalId)
                .HasMaxLength(50);

            builder.Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(d => d.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Address)
                .HasMaxLength(500);

            builder.Property(d => d.EmploymentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.PhotoUrl)
                .HasMaxLength(500);

            builder.Property(d => d.DateOfBirth)
                .HasColumnType("date");

            builder.Property(d => d.StartDate)
                .HasColumnType("date");
        }
    }
}
