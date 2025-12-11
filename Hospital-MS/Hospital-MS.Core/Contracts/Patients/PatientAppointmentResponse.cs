public class PatientAppointmentResponse
{
    public int AppointmentId { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? DoctorName { get; set; }
    public string? ClinicName { get; set; }
    public string? MedicalServiceName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
