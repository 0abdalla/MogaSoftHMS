using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;

namespace Hospital_MS.Core.Models
{
    public sealed class Staff : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        //public string? Specialization { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public StaffStatus Status { get; set; } 
        public string? NationalId { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string? Notes { get; set; }

        //public int? ClinicId { get; set; }
        //public int? DepartmentId { get; set; }
        public int? JobTitleId { get; set; }
        public int? JobTypeId { get; set; }
        public int? JobLevelId { get; set; }
        public int? JobDepartmentId { get; set; }

        // Navigation Property
        //public Clinic? Clinic { get; set; }
        //public Department? Department { get; set; } = default!;
        public JobTitle? JobTitle { get; set; } = default!;
        public JobType? JobType { get; set; } = default!;
        public JobLevel? JobLevel { get; set; } = default!;
        public JobDepartment? JobDepartment { get; set; }
        public ICollection<StaffAttachments> StaffAttachments { get; set; } = new HashSet<StaffAttachments>();
    }
}
