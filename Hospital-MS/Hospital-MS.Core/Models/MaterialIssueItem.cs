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
    public bool IsActive { get; set; } = true;
}
