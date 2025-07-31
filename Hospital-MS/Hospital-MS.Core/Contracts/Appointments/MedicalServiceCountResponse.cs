namespace Hospital_MS.Core.Contracts.Appointments;
public class MedicalServiceCountResponse
{
    public int? MedicalServiceId { get; set; }
    public string? MedicalServiceName { get; set; }
    public int Count { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? Price { get; set; }
}
