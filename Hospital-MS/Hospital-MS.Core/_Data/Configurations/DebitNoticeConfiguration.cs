using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations
{
    public class DebitNoticeConfiguration : IEntityTypeConfiguration<DebitNotice>
    {
        public void Configure(EntityTypeBuilder<DebitNotice> builder)
        {
            builder.ToTable("DebitNotices", schema: "finance");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.CheckNumber)
                .HasMaxLength(50);

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);           
        }
    }
}