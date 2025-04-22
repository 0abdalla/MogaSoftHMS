using Hospital_MS.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class CreateStaffRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly HireDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? NationalId { get; set; }

        public string? MaritalStatus { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string? Notes { get; set; }

        public int DepartmentId { get; set; }
        public int? ClinicId { get; set; }

        public List<IFormFile> Files { get; set; } = [];
    }
}
