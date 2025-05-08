using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital_MS.Core._Data.Configurations.HR
{
    public class JobDepartmentConfiguration : IEntityTypeConfiguration<JobDepartment>
    {
        public void Configure(EntityTypeBuilder<JobDepartment> builder)
        {
            builder.Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(j => j.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Status)
                     .HasConversion(
                     (type) => type.ToString(),
                     (stu) => Enum.Parse<StatusTypes>(stu, true)).HasMaxLength(55);
        }
    }
}
