using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.MedicalServices
{
    public class MedicalServiceRequest
    {
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public string Type { get; set; }
    }
}
