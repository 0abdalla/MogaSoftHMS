using Hospital_MS.Core.Contracts.Admissions;

public class PatientAdmissionsWithAppointmentsResponse
{
    public IReadOnlyList<PatientAdmissionsResponse> Admissions { get; set; } = new List<PatientAdmissionsResponse>();
    public IReadOnlyList<PatientAppointmentResponse> Appointments { get; set; } = new List<PatientAppointmentResponse>();
    public bool HasAppointments { get; set; }
}
