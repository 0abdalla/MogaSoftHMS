using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models
{
    public sealed class Patient : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public DateOnly? DateOfBirth { get; set; } 
        public string? NationalId { get; set; }
        public string? Phone { get; set; } 
        public string? Email { get; set; } 
        public string? Address { get; set; } 
        public string? Nationality { get; set; } 

        public string? EmergencyPhone01 { get; set; }
        public string? EmergencyContact01 { get; set; } 

        public string? EmergencyPhone02 { get; set; } 
        public string? EmergencyContact02 { get; set; } 

        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public Gender? Gender { get; set; }
        public PatientStatus? Status { get; set; }


        public int? InsuranceCompanyId { get; set; }
        public int? InsuranceCategoryId { get; set; }
        public string? InsuranceNumber { get; set; }

        public InsuranceCompany? InsuranceCompany { get; set; }
        public InsuranceCategory? InsuranceCategory { get; set; }
    }
}
