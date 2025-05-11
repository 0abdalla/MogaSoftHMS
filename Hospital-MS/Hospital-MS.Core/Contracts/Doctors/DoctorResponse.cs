using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Doctors
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public int? SpecialtyId { get; set; }
        public int? DepartmentId { get; set; }
        public DateOnly StartDate { get; set; }
        public bool IsActive { get; set; }
        public string? PhotoUrl { get; set; }

        public string Specialty { get; set; }
        public string Department { get; set; }

        public string? Notes { get; set; }
        public string Status { get; set; }
        public string MaritalStatus { get; set; }
        public string? Degree { get; set; }

        //public int? MedicalServiceId { get; set; }
        //public string? MedicalServiceName { get; set; }
        public List<DoctorScheduleResponse> DoctorSchedules { get; set; } = [];
        public List<DoctorMedicalServiceResponse> DoctorMedicalServices { get; set; } = [];

        // Audit properties
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
