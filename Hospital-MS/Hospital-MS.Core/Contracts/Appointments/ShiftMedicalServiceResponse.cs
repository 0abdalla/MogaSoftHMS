namespace Hospital_MS.Core.Contracts.Appointments;
public class ShiftMedicalServiceResponse
{
    public int Id { get; set; }
    public int? MedicalServiceId { get; set; }
    public string? MedicalServiceName { get; set; }
    public int Count { get; set; }
    public decimal? Price { get; set; }
    public decimal? TotalPrice { get; set; }
}
