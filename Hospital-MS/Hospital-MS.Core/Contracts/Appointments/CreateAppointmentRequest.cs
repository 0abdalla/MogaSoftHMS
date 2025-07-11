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
        public string PatientName { get; set; }
        public string? PatientPhone { get; set; }
        public string Gender { get; set; }
        public string? AppointmentType { get; set; }


        //public int? ClinicId { get; set; }
        public int? DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }

        public int? InsuranceCompanyId { get; set; }
        public int? InsuranceCategoryId { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? PaymentMethod { get; set; }

        // بيانات الطوارئ
        public string? EmergencyLevel { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? CompanionPhone { get; set; }

        public List<MedicalServiceModel> MedicalServices { get; set; }
    }

    public class MedicalServiceModel
    {
        public int MedicalServiceId { get; set; }
        public DateOnly AppointmentDate { get; set; }
    }
}
