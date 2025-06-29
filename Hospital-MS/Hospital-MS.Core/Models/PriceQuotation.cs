using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class PriceQuotation : AuditableEntity
{
    public int Id { get; set; }
    public string QuotationNumber { get; set; } = string.Empty;
    public DateTime QuotationDate { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public string? Notes { get; set; }
    public QuotationStatus Status { get; set; } = QuotationStatus.Pending;
    public bool IsActive { get; set; } = true;

    public ICollection<PriceQuotationItem> Items { get; set; } = new HashSet<PriceQuotationItem>();
}
