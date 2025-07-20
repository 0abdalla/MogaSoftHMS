using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Appointments
{
    public class AppointmentsGroupResponse
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string? DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int? DoctorId { get; set; }
        public int PatientId { get; set; }
        public int AppointmentNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string PatientPhone { get; set; }
        public int? ClinicId { get; set; }
        public string? ClinicName { get; set; }
        public int TotalCount { get; set; }
        public string? MedicalServiceName { get; set; }
        public string? RadiologyBodyTypeName { get; set; }
    }

    public class AppointmentsGroupResponseModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int AppointmentNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string PatientPhone { get; set; }
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string MedicalServiceName { get; set; }
        public string RadiologyBodyTypeName { get; set; }
    }
}
