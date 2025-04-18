using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HMS.Infrastructure._Data.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(r => r.Number).IsRequired();

            builder.Property(p => p.Type)
                       .HasConversion(
                       (type) => type.ToString(),
                       (gen) => Enum.Parse<RoomType>(gen, true)).HasMaxLength(55);

            builder.Property(p => p.Status)
                    .HasConversion(
                    (type) => type.ToString(),
                    (gen) => Enum.Parse<RoomStatus>(gen, true)).HasMaxLength(55);
        }
    }
}
