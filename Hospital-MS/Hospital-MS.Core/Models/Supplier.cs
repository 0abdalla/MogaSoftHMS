using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class Supplier : AuditableEntity
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public AccountTree Account { get; set; } = default!;
    public string AccountCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ResponsibleName1 { get; set; } = string.Empty;
    public string? ResponsibleName2 { get; set; }
    public string Phone1 { get; set; } = string.Empty;
    public string? Phone2 { get; set; }
    public string TaxNumber { get; set; } = string.Empty;
    public string? Job { get; set; }
    public string? Fax1 { get; set; }
    public string? Fax2 { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Notes { get; set; }

    public PaymentType PaymentType { get; set; } // Cash, Credit 
    public decimal? CreditLimit { get; set; } // حد الائتمان
    public decimal? CurrentBalance { get; set; } // الرصيد الحالي
}
