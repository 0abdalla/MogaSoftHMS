using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DispensePermission;
public class DispensePermissionResponse
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int FromStoreId { get; set; }
    public string FromStoreName { get; set; }
    public int ToStoreId { get; set; }
    public string ToStoreName { get; set; }
    public decimal Quantity { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Balance { get; set; }
    public string Notes { get; set; }
    public string Status { get; set; }

    public AuditResponse Audit { get; set; }
}
