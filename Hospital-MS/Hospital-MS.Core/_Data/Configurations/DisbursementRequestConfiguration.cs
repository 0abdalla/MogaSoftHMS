using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;

public class DisbursementRequestConfiguration : IEntityTypeConfiguration<DisbursementRequest>
{
    public void Configure(EntityTypeBuilder<DisbursementRequest> builder)
    {
        builder.ToTable("DisbursementRequests", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);
    }
}