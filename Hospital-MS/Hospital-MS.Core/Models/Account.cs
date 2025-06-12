using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;

[Table("Accounts",Schema = "finance")]
public class Account : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}
