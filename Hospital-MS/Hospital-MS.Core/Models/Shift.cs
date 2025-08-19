using System.ComponentModel.DataAnnotations;

namespace Hospital_MS.Core.Models;
public class Shift
{
    public int Id { get; set; }
    public decimal? TotalAmount { get; set; }
    [MaxLength(350)]
    public string ClosedBy { get; set; } = string.Empty;
    public DateTime? ClosedAt { get; set; }
    public DateTime OpenedAt { get; set; }

    public List<ShiftMedicalService> MedicalServices { get; set; } = new();
}
