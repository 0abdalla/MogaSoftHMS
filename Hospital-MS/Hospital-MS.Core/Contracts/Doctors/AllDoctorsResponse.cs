﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Doctors
{
    public class AllDoctorsResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public int? MedicalServiceId { get; set; }
        public double? Price { get; set; }
        public List<DoctorMedicalServiceResponse> MedicalServices { get; set; } = [];
        public List<DoctorScheduleResponse> DoctorSchedules { get; set; } = [];

    }
}
