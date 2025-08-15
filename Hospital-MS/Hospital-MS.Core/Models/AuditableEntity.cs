using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class AuditableEntity
    {
        [ForeignKey(nameof(CreatedBy))]
        public string? CreatedById { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public string? UpdatedById { get; set; }

        public ApplicationUser? CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
