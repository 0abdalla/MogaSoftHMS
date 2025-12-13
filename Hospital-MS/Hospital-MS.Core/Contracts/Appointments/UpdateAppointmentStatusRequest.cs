using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class UpdateAppointmentStatusRequest
    {
        public AppointmentStatus Status { get; set; }
    }
}
