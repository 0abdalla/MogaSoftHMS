using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
