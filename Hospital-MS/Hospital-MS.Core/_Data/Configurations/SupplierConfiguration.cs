using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            // Set schema
            builder.ToTable("Suppliers", schema: "finance");

            builder.Property(s => s.AccountCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.ResponsibleName1)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.ResponsibleName2)
                .HasMaxLength(100);

            builder.Property(s => s.Phone1)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(s => s.Phone2)
                .HasMaxLength(15);

            builder.Property(s => s.TaxNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Job)
                .HasMaxLength(100);

            builder.Property(s => s.Fax1)
                .HasMaxLength(20);

            builder.Property(s => s.Fax2)
                .HasMaxLength(20);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Website)
                .HasMaxLength(200);

            builder.Property(s => s.Notes)
                .HasMaxLength(500);
   
            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(s => s.AccountCode)
                .IsUnique();

            builder.HasIndex(s => s.Email)
                .IsUnique();

            builder.HasIndex(s => s.Phone1);

            builder.HasIndex(s => s.TaxNumber)
                .IsUnique();
        }
    }
}