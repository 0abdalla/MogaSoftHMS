using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class CreateAppointmentRequest
    {
        public string PatientName { get; set; } = default!;
        public string? PatientPhone { get; set; }
        public Gender Gender { get; set; }

        public AppointmentType AppointmentType { get; set; }
        public DateTime AppointmentDate { get; set; }

        public int? DoctorId { get; set; }

        public PaymentMethodType? PaymentMethod { get; set; }

        public List<int>? MedicalServiceIds { get; set; }

        public int? InsuranceCompanyId { get; set; }
        public int? InsuranceCategoryId { get; set; }
        public string? InsuranceNumber { get; set; }

        public string? ChiefComplaint { get; set; }
        public string? Notes { get; set; }
    }

    public class MedicalServiceModel
    {
        public DateTime AppointmentDate { get; set; }
        public List<int> MedicalServiceIds { get; set; }
        public string AppointmentType { get; set; }
    }
}
