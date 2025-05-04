using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class MedicalService
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0M;
        public string Type { get; set; } = string.Empty;
 
        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
