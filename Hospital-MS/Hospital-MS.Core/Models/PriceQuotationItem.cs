using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class PriceQuotationItem : AuditableEntity
{
    public int Id { get; set; }
    public int PriceQuotationId { get; set; }
    public PriceQuotation PriceQuotation { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}