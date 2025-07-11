using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_MS.Core.Models;
[Table("AccountingGuidance",Schema ="finance")]
public class AccountingGuidance : AuditableEntity // التوجيه المحاسبي
{
    public int Id { get; set; }
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true; 
}
