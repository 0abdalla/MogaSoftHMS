using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;
public class SupplyReceipt : AuditableEntity
{
    public int Id { get; set; }
    public string SupplyReceiptNumber { get; set; }
    public int TreasuryId { get; set; }
    public DateTime? Date { get; set; }
    public string? ReceivedFrom { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public int? CostCenterId { get; set; }
    public int? AccountId { get; set; }
    public int? DailyRestrictionId { get; set; }

    public AccountTree? Account { get; set; }
    public Treasury Treasury { get; set; } = default!;
    public CostCenterTree? CostCenter { get; set; } = default!;
    public DailyRestriction? DailyRestriction { get; set; } = default!;

    //public Invoice? Invoice { get; set; }
    //public int? InvoiceId { get; set; }
    //public bool HasInvoice { get; set; } = false;
    public PaymentType PaymentType { get; set; }
}
