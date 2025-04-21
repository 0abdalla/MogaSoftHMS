using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class PatientMedicalHistory : AuditableEntity
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;

    }
}
