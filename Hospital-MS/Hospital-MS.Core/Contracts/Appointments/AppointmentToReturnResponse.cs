using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class AppointmentToReturnResponse
    {
        public string? PatientName { get; set; }
        public string? PatientPhone { get; set; }

        public DateOnly? AppointmentDate { get; set; }
        public string? DoctorName { get; set; }

        public int AppointmentNumber { get; set; }
        public string? MedicalServiceName { get; set; }

    }
}
