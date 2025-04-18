using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class BedConfiguration : IEntityTypeConfiguration<Bed>
    {
        public void Configure(EntityTypeBuilder<Bed> builder)
        {
            builder.Property(b => b.Number)
               .IsRequired();

            builder.Property(b => b.Status)
                  .HasConversion(
                  (type) => type.ToString(),
                  (gen) => Enum.Parse<BedStatus>(gen, true)).HasMaxLength(55);
        }
    }
}
