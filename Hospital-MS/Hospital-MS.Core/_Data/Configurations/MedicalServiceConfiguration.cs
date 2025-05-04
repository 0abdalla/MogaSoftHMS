using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class MedicalServiceConfiguration : IEntityTypeConfiguration<MedicalService>
    {
        public void Configure(EntityTypeBuilder<MedicalService> builder)
        {
            builder.Property(ms => ms.Name)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(ms => ms.Type)
               .IsRequired()
               .HasMaxLength(70);

            builder.Property(ms => ms.Price)
                .HasColumnType("decimal(18,2)");

        }
    }
}
