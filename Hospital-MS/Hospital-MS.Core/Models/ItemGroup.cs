namespace Hospital_MS.Core.Models;

public class ItemGroup : AuditableEntity
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Item> Items { get; set; } = new HashSet<Item>();
}