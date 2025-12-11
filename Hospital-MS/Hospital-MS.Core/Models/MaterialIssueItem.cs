using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class MaterialIssueItem : AuditableEntity
{
    public int Id { get; set; }
    public int MaterialIssuePermissionId { get; set; }
    public MaterialIssuePermission MaterialIssuePermission { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public string Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public ICollection<MaterialIssueBatch> Batches { get; set; } = new List<MaterialIssueBatch>();

}

public class MaterialIssueBatch : AuditableEntity
{
    public int Id { get; set; }
    public int MaterialIssueItemId { get; set; }
    public MaterialIssueItem MaterialIssueItem { get; set; } = default!;

    public int ItemBatchId { get; set; }
    public ItemBatch ItemBatch { get; set; } = default!;
    public string BatchNumber { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
}