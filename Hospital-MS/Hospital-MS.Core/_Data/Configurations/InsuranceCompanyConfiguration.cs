using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class InsuranceCompanyConfiguration : IEntityTypeConfiguration<InsuranceCompany>
    {
        public void Configure(EntityTypeBuilder<InsuranceCompany> builder)
        {
            builder.Property(i => i.Name).IsRequired()
                .HasMaxLength(500);

            builder.Property(ic => ic.Email)
                .HasMaxLength(100);

            builder.Property(ic => ic.Phone)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(ic => ic.Code)
                .HasMaxLength(50);

            builder.Property(ic => ic.ContractStartDate)
                .IsRequired().HasColumnType("date"); ;

            builder.Property(ic => ic.ContractEndDate)
                .IsRequired().HasColumnType("date"); ;

        }
    }
}
