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
    public string AccountCode { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string ResponsibleName1 { get; set; }
    public string? ResponsibleName2 { get; set; }
    public string Phone1 { get; set; }
    public string? Phone2 { get; set; }
    public string TaxNumber { get; set; }
    public string? Job { get; set; }
    public string? Fax1 { get; set; }
    public string? Fax2 { get; set; }
    public string Email { get; set; }
    public string? Website { get; set; }
    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;
}
