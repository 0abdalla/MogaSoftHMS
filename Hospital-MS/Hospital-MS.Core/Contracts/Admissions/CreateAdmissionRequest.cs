using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Admissions
{
    public class CreateAdmissionRequest
    {
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
        public DateOnly? PatientBirthDate { get; set; }
        public string? PatientNationalId { get; set; }
        public string? PatientAddress { get; set; }
        public string PatientStatus { get; set; }

        public string? EmergencyPhone01 { get; set; } 
        public string? EmergencyContact01 { get; set; } 

        public string? EmergencyPhone02 { get; set; } 
        public string? EmergencyContact02 { get; set; } 

        public int DepartmentId { get; set; }
        public int DoctorId { get; set; }
        public int RoomId { get; set; }
        public int BedId { get; set; }

        public int? InsuranceCompanyId { get; set; }
        public int? InsuranceCategoryId { get; set; }
        public string? InsuranceNumber { get; set; }


        public string? HealthStatus { get; set; }
        public string? InitialDiagnosis { get; set; }

        public bool HasCompanion { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? CompanionPhone { get; set; }

        public string? Notes { get; set; }

    }
}
