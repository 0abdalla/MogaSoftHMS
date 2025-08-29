using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
        builder.ToTable("PurchaseRequests", schema: "finance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RequestNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Purpose).HasMaxLength(500);
        builder.Property(x => x.Notes).HasMaxLength(500);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasOne(x => x.Store).WithMany().HasForeignKey(x => x.StoreId);

        // PriceQuotation
        builder.HasOne(pr => pr.PriceQuotation)
               .WithOne()
               .HasForeignKey<PurchaseRequest>(pr => pr.PriceQuotationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}