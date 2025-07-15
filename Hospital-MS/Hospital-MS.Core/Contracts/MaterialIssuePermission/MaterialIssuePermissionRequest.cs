using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.MaterialIssuePermission;
public class MaterialIssuePermissionRequest
{
   // public string? DocumentNumber { get; set; }
    public DateTime PermissionDate { get; set; }
    public int StoreId { get; set; }
    public int BranchId { get; set; }
    public string? Notes { get; set; }
    public int? DisbursementRequestId { get; set; } 
    public List<MaterialIssueItemRequest> Items { get; set; } = [];
}
