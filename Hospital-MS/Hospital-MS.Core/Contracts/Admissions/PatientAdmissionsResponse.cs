namespace Hospital_MS.Core.Contracts.Admissions
{
    public class PatientAdmissionsResponse
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhoneNumber { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; }
        public string? HealthStatus { get; set; }
        public string? Notes { get; set; }
        public string? DoctorName { get; set; }
        public int? RoomNumber { get; set; }
        public int? BedNumber { get; set; }
        public string? DepartmentName { get; set; }
        public string? PatientStatus { get; set; }

        public string? NationalId { get; set; }
        public string? Gender { get; set; }
    }
}
