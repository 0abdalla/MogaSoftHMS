using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Clinics
{
    public class CreateClinicRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
