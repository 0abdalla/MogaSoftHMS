using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DispensePermission;
public class DispensePermissionRequest
{
    public DateOnly Date { get; set; }
    public int FromStoreId { get; set; }
    public int ToStoreId { get; set; }
    public decimal Quantity { get; set ; }
    public int ItemId { get; set; }
    public string Notes { get; set; } 
}
