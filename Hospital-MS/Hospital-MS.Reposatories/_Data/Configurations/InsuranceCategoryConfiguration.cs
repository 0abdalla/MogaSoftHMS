using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class InsuranceCategoryConfiguration : IEntityTypeConfiguration<InsuranceCategory>
    {
        public void Configure(EntityTypeBuilder<InsuranceCategory> builder)
        {
            builder.Property(i => i.Name).IsRequired()
                  .HasMaxLength(500);
        }
    }
}
