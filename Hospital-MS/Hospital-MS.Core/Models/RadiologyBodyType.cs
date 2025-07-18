using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class RadiologyBodyType
    {
        public int Id { get; set; }
        public int MedicalServiceId { get; set; }
        public string Name { get; set; }
    }
}
