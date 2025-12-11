namespace Hospital_MS.Core.Contracts.Appointments
{
    public class AppointmentToReturnResponse
    {
        public int AppointmentId { get; set; }
        public int AppointmentNumber { get; set; }
        public string? MedicalServiceName { get; set; }
        public string? DoctorName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public decimal TotalPrice { get; set; }

        public string? PatientName { get; set; }
        public string? PatientPhone { get; set; }
        public List<MedicalServiceResponse> MedicalServices { get; set; } = new();

        public bool? IsDeleted { get; set; }

    }

    public class MedicalServiceResponse
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
