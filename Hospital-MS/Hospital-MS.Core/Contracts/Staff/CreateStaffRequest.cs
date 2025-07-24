using Microsoft.AspNetCore.Http;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class CreateStaffRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Code { get; set; } // كود البصمه
        //public string Specialization { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public string Status { get; set; } = string.Empty;
        //public string Type { get; set; } = string.Empty;
        public string? NationalId { get; set; }

        public string? MaritalStatus { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string? Notes { get; set; }

        //public int DepartmentId { get; set; }
        //public int? ClinicId { get; set; }

        public List<IFormFile> Files { get; set; } = [];

        public bool IsAuthorized { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public int? JobTitleId { get; set; }
        public int? JobTypeId { get; set; }
        public int? JobLevelId { get; set; }
        public int? JobDepartmentId { get; set; }

        public int? BranchId { get; set; }
        public double? BasicSalary { get; set; }
        public int? Tax { get; set; }
        public int? Insurance { get; set; }
        public int? VacationDays { get; set; }

        public decimal VariableSalary { get; set; }
        public string? VisaCode { get; set; }
        public decimal Allowances { get; set; } // البدلات
        public decimal Rewards { get; set; } // المكافأت
    }
}
