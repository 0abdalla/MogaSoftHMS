using Hospital_MS.Core.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.MainGroups;
public class MainGroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public AuditResponse Audit { get; set; } = new();
}
