﻿using Hospital_MS.Core.Enums;
using System;
using System.Collections;
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
        public string FullName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public StaffStatus Status { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string? Degree { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public double? Price { get; set; }
        public int? SpecialtyId { get; set; }
        public int? DepartmentId { get; set; }
        public DateOnly StartDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public string? PhotoUrl { get; set; }
        //public int? MedicalServiceId { get; set; }
        //public MedicalService? MedicalService { get; set; } = null!;


        // Navigation Properties
        public Specialty? Specialty { get; set; } = null!;
        public Department? Department { get; set; } = null!;
        public ICollection<DoctorMedicalService> DoctorMedicalServices { get; set; } = new List<DoctorMedicalService>();
        public ICollection<DoctorRating>? Ratings { get; set; } = new HashSet<DoctorRating>();
        public ICollection<DoctorSchedule> Schedules { get; set; } = new HashSet<DoctorSchedule>();
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<Admission> Admissions { get; set; } = new HashSet<Admission>();
    }
}
