using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class Department : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Admission> Admissions { get; set; } = new HashSet<Admission>();
        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
