namespace Hospital_MS.Core.Contracts.Appointments;
public class ShiftResponse
{
    public int Id { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? ClosedBy { get; set; }
    public List<ShiftMedicalServiceResponse> MedicalServices { get; set; } = [];
}
