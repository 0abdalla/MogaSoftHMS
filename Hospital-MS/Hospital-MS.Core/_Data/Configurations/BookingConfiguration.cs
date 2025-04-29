using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {

            builder.HasIndex(b => b.BookingNumber)
                .IsUnique();

            builder.Property(b => b.BookingNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("BookingNumber");

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.Currency)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("EGP");

            builder.Property(b => b.Notes)
                .HasMaxLength(500);

            builder.Property(b => b.Price)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0M);

            builder.Property(b => b.Status)
                    .HasConversion(
                    (type) => type.ToString(),
                    (gen) => Enum.Parse<BookingStatus>(gen, true)).HasMaxLength(55);

        }
    }
}
