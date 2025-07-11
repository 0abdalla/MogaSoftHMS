namespace Hospital_MS.Core.Models;

public class ItemGroup : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public int? MainGroupId { get; set; }
    public MainGroup? MainGroup { get; set; } = default!;
    public ICollection<Item> Items { get; set; } = new HashSet<Item>();
}