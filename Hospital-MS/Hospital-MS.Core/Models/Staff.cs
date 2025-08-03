using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models.HR;

namespace Hospital_MS.Core.Models
{
    public class Staff : AuditableEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public StaffStatus Status { get; set; }
        public string? NationalId { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string? Notes { get; set; }
        public int? JobTitleId { get; set; }
        public int? JobTypeId { get; set; }
        public int? JobLevelId { get; set; }
        public int? JobDepartmentId { get; set; }
        public int? BranchId { get; set; }
        public double? BasicSalary { get; set; }
        public int? Tax { get; set; }
        public int? Insurance { get; set; }
        public int? VacationDays { get; set; }
        public JobTitle? JobTitle { get; set; } = default!;
        public JobType? JobType { get; set; } = default!;
        public JobLevel? JobLevel { get; set; } = default!;
        public JobDepartment? JobDepartment { get; set; }
        public Branch? Branch { get; set; }
        public ICollection<StaffAttachments> StaffAttachments { get; set; } = new HashSet<StaffAttachments>();
        public ICollection<EmployeeAdvance> EmployeeAdvances { get; set; } = new HashSet<EmployeeAdvance>();
        public ICollection<AttendanceSalary> AttendaceSalaries { get; set; } = new HashSet<AttendanceSalary>();

        

        public decimal VariableSalary { get; set; }
        public string? VisaCode { get; set; }
        public decimal Allowances { get; set; } // البدلات
        public decimal Rewards { get; set; } // المكافأت
    }
}
