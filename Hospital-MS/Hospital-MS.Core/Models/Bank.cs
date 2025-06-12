using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;

public class Bank : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } =string.Empty;
    public string? Code { get; set; } 
    public string? AccountNumber { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; } = 0.0m;

    public bool IsActive { get; set; } = true;
}
