using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class UpdatePatientStatusInEmergencyRequest
    {
        public string NewStatus { get; set; }
    }
}
