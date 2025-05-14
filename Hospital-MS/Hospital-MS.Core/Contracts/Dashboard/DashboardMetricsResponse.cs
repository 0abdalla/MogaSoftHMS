using Hospital_MS.Core.Contracts.Dashboard;

public class DashboardMetricsResponse
{
    public decimal BedOccupancyRate { get; set; }
    public int ActiveDoctors { get; set; }
    public int CurrentPatients { get; set; }
    public int AppointmentsCount { get; set; }
    public decimal CompletedAppointmentsRate { get; set; }

    public Dictionary<string,int> CurrentMonthAppointments { get; set; } = [];
    public Dictionary<string, int> PreviousMonthAppointments { get; set; } = [];
    public List<TopDoctorMetric> TopDoctors { get; set; }

}