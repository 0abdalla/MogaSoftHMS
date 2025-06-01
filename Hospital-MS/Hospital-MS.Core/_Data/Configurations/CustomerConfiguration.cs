using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.NameEn)
            .HasMaxLength(100);

        builder.Property(x => x.ResponsibleName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ResponsibleName2)
            .HasMaxLength(100);

        builder.Property(x => x.CustomerType)
            .HasMaxLength(50);

        builder.Property(x => x.Job)
            .HasMaxLength(100);

        builder.Property(x => x.Region)
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(x => x.Phone2)
            .HasMaxLength(15);

        builder.Property(x => x.Telephone)
            .HasMaxLength(15);

        builder.Property(x => x.Telephone2)
            .HasMaxLength(15);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(x => x.PaymentResponsible)
            .HasMaxLength(100);

        builder.Property(x => x.CreditLimit)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Phone);
        builder.HasIndex(x => x.Email);
    }
}
