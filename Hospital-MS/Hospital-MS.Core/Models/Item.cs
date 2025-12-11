namespace Hospital_MS.Core.Models;

public class Item : AuditableEntity
{
    public int Id { get; set; }
    public string NameAR { get; set; } = string.Empty;
    public string NameEN { get; set; } = string.Empty;
    public int OrderLimit { get; set; }
    public decimal Cost { get; set; }
    public bool IsConsumable { get; set; }
    public bool IsMedicine { get; set; }
    public bool IsAsset { get; set; }
    public decimal SalesTax { get; set; }
    public decimal Price { get; set; }
    public decimal PriceAfterTax { get; set; }
    public bool HasBarcode { get; set; }
    public bool IsGroupHead { get; set; }
    public long OpeningBalance { get; set; }
    public long CurrentBalance { get; set; }
    public decimal AverageCost { get; set; } // المتوسط المرجح
    public int? UnitId { get; set; }
    public int? GroupId { get; set; }
    public ItemGroup? Group { get; set; } = default!;
    public ItemUnit? Unit { get; set; } = default!;
    public ICollection<ItemBatch> Batches { get; set; } = new List<ItemBatch>();
    public bool HasExpiryDate { get; set; }
}

public class ItemBatch : AuditableEntity
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public string BatchNumber { get; set; } = string.Empty;
    public DateTime? ProductionDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int StoreId { get; set; }
    public decimal Quantity { get; set; }
    public decimal RemainingQuantity { get; set; }
    public decimal UnitCost { get; set; }
    public bool IsOpeningBalance { get; set; }
    public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now;

    public int? ReceiptPermissionId { get; set; }
    public ReceiptPermission? ReceiptPermission { get; set; }
    public Store Store { get; set; }
}
public class ItemAverageCost : AuditableEntity
{
    public int Id { get; set; }

    public int ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public decimal AverageCost { get; set; }
    public DateTime AverageDate { get; set; }
}
