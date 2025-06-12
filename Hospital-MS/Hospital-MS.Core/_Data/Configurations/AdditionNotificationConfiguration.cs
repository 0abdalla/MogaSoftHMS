using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations;
public class AdditionNotificationConfiguration : IEntityTypeConfiguration<AdditionNotice>
{
    public void Configure(EntityTypeBuilder<AdditionNotice> builder)
    {
        builder.ToTable("AdditionNotifications", schema: "finance");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.CheckNumber)
            .HasMaxLength(50);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}