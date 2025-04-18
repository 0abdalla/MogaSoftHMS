using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class UpdateAppointmentRequest
    {
        public int DoctorId { get; set; }
        public int ClinicId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string AppointmentType { get; set; }
    }
}
