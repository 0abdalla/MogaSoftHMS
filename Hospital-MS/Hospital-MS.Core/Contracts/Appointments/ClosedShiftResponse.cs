namespace Hospital_MS.Core.Contracts.Appointments;
public class ClosedShiftResponse
{
    public List<MedicalServiceCountResponse> MedicalServices { get; set; } = new();
    public decimal? TotalAmount { get; set; }
    public string ClosedBy { get; set; } = string.Empty;
    public DateTime ClosedAt { get; set; }
}
