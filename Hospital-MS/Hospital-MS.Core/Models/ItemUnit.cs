namespace Hospital_MS.Core.Models;
public class ItemUnit : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    //public ICollection<Item> Items { get; set; } = new HashSet<Item>();
}