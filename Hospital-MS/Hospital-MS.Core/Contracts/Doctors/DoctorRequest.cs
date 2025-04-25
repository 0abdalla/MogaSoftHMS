using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Hospital_MS.Core.Contracts.Doctors
{
    public class DoctorRequest
    {
        public string? NationalId { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; } 
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int SpecialtyId { get; set; }
        public int DepartmentId { get; set; }
        public DateOnly StartDate { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
        public string MaritalStatus { get; set; }
        public string? Degree { get; set; }
        public IFormFile? Photo { get; set; }
        //public bool IsActive { get; set; }

        public List<DoctorScheduleRequest>? DoctorSchedules { get; set; } = [];
    }
}
