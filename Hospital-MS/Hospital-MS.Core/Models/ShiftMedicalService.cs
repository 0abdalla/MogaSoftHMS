using System.ComponentModel.DataAnnotations;

namespace Hospital_MS.Core.Models;
public class ShiftMedicalService
{
    public int Id { get; set; }
    public int ShiftId { get; set; }
    public Shift Shift { get; set; } = default!;

    public int? MedicalServiceId { get; set; }
    [MaxLength(350)]
    public string? MedicalServiceName { get; set; } = string.Empty;

    public int Count { get; set; }
    public decimal? Price { get; set; }
    public decimal? TotalPrice { get; set; }
}