using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string? PatientPhone { get; set; }
        public string Gender { get; set; }
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; } = string.Empty;
        public int? ClinicId { get; set; }
        public string? ClinicName { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? PaymentMethod { get; set; } 
        public string? Type { get; set; }
        // بيانات الطوارئ
        public string? EmergencyLevel { get; set; }          // درجة الخطورة
        public string? CompanionName { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? CompanionPhone { get; set; }
        public int AppointmentNumber { get; set; }

        public List<MedicalServicesModel> MedicalServices { get; set; }

    }

    public class MedicalServicesModel
    {
        public int? MedicalServiceId { get; set; }
        public string? MedicalServiceName { get; set; }
        public string? MedicalServiceType { get; set; }
        public decimal MedicalServicePrice { get; set; }
        public DateOnly MedicalServiceDate { get; set; }
        public string? DoctorName { get; set; }
    }
}
