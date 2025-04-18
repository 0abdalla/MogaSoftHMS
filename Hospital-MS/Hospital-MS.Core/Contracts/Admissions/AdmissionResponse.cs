using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Admissions
{
    public class AdmissionResponse
    {
        // Patient Information
        public string PatientName { get; set; } = string.Empty;
        public int PatientId { get; set; }
        //public string? MedicalNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string PatientStatus { get; set; }

        // Admission Details
        public DateTime AdmissionDate { get; set; }
        public int? RoomNumber { get; set; }
        public int? BedNumber { get; set; }
        public string? DepartmentName { get; set; }

        // Doctor Information
        public string? DoctorName { get; set; }

        // Insurance Information
        public string? InsuranceCompanyName { get; set; }
        public string? InsuranceCategoryName { get; set; }
        public string? InsuranceNumber { get; set; }

        // Emergency Contacts
        public string? EmergencyContact01 { get; set; }
        public string? EmergencyPhone01 { get; set; }
        public string? EmergencyContact02 { get; set; }
        public string? EmergencyPhone02 { get; set; }

        // Medical Information
        public string? HealthStatus { get; set; }
        public string? InitialDiagnosis { get; set; }

        // Additional Information
        public bool HasCompanion { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionPhone { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? Notes { get; set; }

        // Audit Information
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
