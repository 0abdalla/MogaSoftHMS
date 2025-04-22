using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    public class Doctor : AuditableEntity
    {
        public int Id { get; set; }
        public string? NationalId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; } 
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int SpecialtyId { get; set; }
        public int DepartmentId { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? PhotoUrl { get; set; }

        // Navigation Properties
        public Specialty Specialty { get; set; } = null!;
        public Department Department { get; set; } = null!;
        public ICollection<DoctorRating> Ratings { get; set; } = new HashSet<DoctorRating>();
    }
}
