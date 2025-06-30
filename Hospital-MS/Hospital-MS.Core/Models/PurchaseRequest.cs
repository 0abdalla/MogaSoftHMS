using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class PurchaseRequest : AuditableEntity
{
    public int Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public int StoreId { get; set; }
    public Store Store { get; set; } = null!;
    public PurchaseStatus Status { get; set; } = PurchaseStatus.Pending;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;


    public ICollection<PurchaseRequestItem> Items { get; set; } = new HashSet<PurchaseRequestItem>();
}