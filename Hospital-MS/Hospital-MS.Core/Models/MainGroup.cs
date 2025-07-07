using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_MS.Core.Models;

[Table("MainGroups", Schema = "finance")]
public class MainGroup : AuditableEntity // المجموعة الرئيسية
{
    public int Id { get; set; }
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<ItemGroup> ItemGroups { get; set; } = new HashSet<ItemGroup>();
}
