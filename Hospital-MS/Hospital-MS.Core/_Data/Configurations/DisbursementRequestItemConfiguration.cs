using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class DisbursementRequestItemConfiguration : IEntityTypeConfiguration<DisbursementRequestItem>
{
    public void Configure(EntityTypeBuilder<DisbursementRequestItem> builder)
    {
        builder.ToTable("DisbursementRequestItems", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .IsRequired();
    }
}