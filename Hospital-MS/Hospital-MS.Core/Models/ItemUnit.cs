namespace Hospital_MS.Core.Models;
public class ItemUnit : AuditableEntity
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Item> Items { get; set; } = new HashSet<Item>();
}