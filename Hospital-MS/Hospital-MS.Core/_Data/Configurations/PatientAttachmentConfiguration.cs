using Hospital_MS.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Reposatories._Data.Configurations
{
    public class PatientAttachmentConfiguration : IEntityTypeConfiguration<PatientAttachment>
    {
        public void Configure(EntityTypeBuilder<PatientAttachment> builder)
        {
            builder.Property(p => p.AttachmentUrl)
                .IsRequired()
                .HasMaxLength(600);
        }
    }
}
