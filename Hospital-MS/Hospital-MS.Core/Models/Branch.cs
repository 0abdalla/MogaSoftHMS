using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class Branch : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; } 
    public bool IsActive { get; set; } = true;
    // Navigation properties
    public ICollection<Treasury> Treasuries { get; set; } = new List<Treasury>();
}
