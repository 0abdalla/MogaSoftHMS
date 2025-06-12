using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.ToTable("Banks", schema: "finance");

            builder.HasKey(x => x.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Code)
                .HasMaxLength(50);

            builder.Property(b => b.AccountNumber)
                .HasMaxLength(50);

            builder.Property(b => b.Currency)
                .HasMaxLength(100);

            builder.Property(b => b.InitialBalance)
                .HasColumnType("decimal(18,2)");                

            builder.Property(b => b.IsActive)
                .HasDefaultValue(true);
        }
    }
}