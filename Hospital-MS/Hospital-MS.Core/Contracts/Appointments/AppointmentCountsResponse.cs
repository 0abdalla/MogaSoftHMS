using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class AppointmentCountsResponse 
    {
        public int EmergencyCount { get; set; }
        public int ScreeningCount { get; set; }
        public int RadiologyCount { get; set; }
        public int SurgeryCount { get; set; }
        public int ConsultationCount { get; set; }
        public int GeneralCount { get; set; }
    }
}
