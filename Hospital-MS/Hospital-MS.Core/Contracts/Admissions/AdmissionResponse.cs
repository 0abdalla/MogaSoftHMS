namespace Hospital_MS.Core.Contracts.Admissions
{
    public class AdmissionResponse
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public int PatientId { get; set; }
        //public string? MedicalNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string PatientStatus { get; set; }
        public string AdmissionType { get; set; } = string.Empty;
        public DateTime? DischargeDate { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int? RoomNumber { get; set; }
        public int? BedNumber { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DoctorId { get; set; }
        public int RoomId { get; set; }
        public int RoomName { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public int BedId { get; set; }
        public string? DoctorName { get; set; }
        public string? InsuranceCompanyName { get; set; }
        public string? InsuranceCategoryName { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? EmergencyContact01 { get; set; }
        public string? EmergencyPhone01 { get; set; }
        public string? EmergencyContact02 { get; set; }
        public string? EmergencyPhone02 { get; set; }
        public string? HealthStatus { get; set; }
        public string? InitialDiagnosis { get; set; }
        public bool HasCompanion { get; set; }
        public string? CompanionName { get; set; }
        public string? CompanionPhone { get; set; }
        public string? CompanionNationalId { get; set; }
        public string? Notes { get; set; }
        //public string? surgeryType { get; set; }
        
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
