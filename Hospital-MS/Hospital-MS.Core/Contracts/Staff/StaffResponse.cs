﻿using Hospital_MS.Core.Contracts.Common;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class StaffResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? NationalId { get; set; }
        public int? ClinicId { get; set; }
        public string? ClinicName { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }

        public string? MaritalStatus { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string? Notes { get; set; }

        public List<string> AttachmentsUrls { get; set; } = new List<string>();

        public int? JobTitleId { get; set; }
        public string? JobTitleName { get; set; }
        public int? JobTypeId { get; set; }
        public string? JobTypeName { get; set; }
        public int? JobLevelId { get; set; }
        public string? JobLevelName { get; set; }
        public int? JobDepartmentId { get; set; }
        public string? JobDepartmentName { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public decimal VariableSalary { get; set; }
        public string? VisaCode { get; set; }
        public decimal Allowances { get; set; } // البدلات
        public decimal Rewards { get; set; } // المكافأت

        public AuditResponse Audit { get; set; }

    }
}
