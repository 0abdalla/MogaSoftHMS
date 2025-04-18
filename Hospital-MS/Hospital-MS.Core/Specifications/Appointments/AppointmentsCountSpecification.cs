using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Appointments
{
    public class AppointmentsCountSpecification : BaseSpecification<Appointment>
    {
        public AppointmentsCountSpecification(GetAppointmentsRequest request)
            : base(x => (string.IsNullOrEmpty(request.Search) ||
                        x.Patient.FullName.ToLower().Contains(request.Search) ||
                        x.Patient.Phone.ToLower().Contains(request.Search)) &&
                        (string.IsNullOrEmpty(request.Type) ||
                        x.Type == Enum.Parse<AppointmentType>(request.Type))
            )
        {

        }
    }
}
