namespace Hospital_MS.Core.Models;

public class DispensePermission : AuditableEntity
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int FromStoreId { get; set; }
    public int ToStoreId { get; set; }
    public decimal Quantity { get; set; }
    public int ItemId { get; set; }
    public decimal Balance { get; set; }
    public string Notes { get; set; }
    public string Status { get; set; } = "Pending";

    public Store FromStore { get; set; }
    public Store ToStore { get; set; }
    public Item Item { get; set; }
}