using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core._Data.Configurations;
public class PriceQuotationConfiguration : IEntityTypeConfiguration<PriceQuotation>
{
    public void Configure(EntityTypeBuilder<PriceQuotation> builder)
    {
        builder.ToTable("PriceQuotations", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QuotationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
