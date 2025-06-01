using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Items;
public class ItemResponse
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int? UnitId { get; set; }
    public string UnitName { get; set; }
    public int? GroupId { get; set; }
    public string GroupName { get; set; }
    public decimal OrderLimit { get; set; }
    public decimal Cost { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal SalesTax { get; set; }
    public decimal Price { get; set; }
    public decimal PriceAfterTax { get; set; }
    public bool HasBarcode { get; set; }
    public int? TypeId { get; set; }
    public string? TypeName { get; set; }
    public bool IsActive { get; set; }

    public AuditResponse Audit { get; set; }
}
