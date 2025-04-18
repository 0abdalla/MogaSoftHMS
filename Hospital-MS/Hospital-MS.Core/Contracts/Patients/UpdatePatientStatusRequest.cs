using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class UpdatePatientStatusRequest
    {
        public string NewStatus { get; set; }
        public string? Notes { get; set; } 
    }
}
