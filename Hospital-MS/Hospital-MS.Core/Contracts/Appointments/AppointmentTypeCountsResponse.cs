namespace Hospital_MS.Core.Contracts.Appointments;
public class AppointmentTypeCountsResponse
{
    public int TotalAppointments { get; set; }
    public int EmergencyAppointments { get; set; }
    public int GeneralAppointments { get; set; }
    public int ConsultationAppointments { get; set; }
    public int SurgeryAppointments { get; set; }
    public int ScreeningAppointments { get; set; }
    public int RadiologyAppointments { get; set; }
}
