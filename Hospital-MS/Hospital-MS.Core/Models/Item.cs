namespace Hospital_MS.Core.Models;

public class Item : AuditableEntity // الصنف
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    //public int? UnitId { get; set; }
    public string? Unit { get; set; }
    public int? GroupId { get; set; }
    public decimal OrderLimit { get; set; }
    public decimal Cost { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal SalesTax { get; set; }
    public decimal Price { get; set; }
    public decimal PriceAfterTax { get; set; }
    public bool HasBarcode { get; set; }
    public int? TypeId { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsGroupHead { get; set; }

    public ItemGroup? Group { get; set; } = default!;
    //public ItemUnit? Unit { get; set; } = default!;
    public ItemType? Type { get; set; } = default!;
}