using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Items;
public class ItemRequest
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    //public int UnitId { get; set; }
    public string? Unit { get; set; }
    public int? GroupId { get; set; }
    public decimal OrderLimit { get; set; }
    public decimal Cost { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal SalesTax { get; set; }
    public decimal Price { get; set; }
    public bool HasBarcode { get; set; }
    public int? TypeId { get; set; }
}
