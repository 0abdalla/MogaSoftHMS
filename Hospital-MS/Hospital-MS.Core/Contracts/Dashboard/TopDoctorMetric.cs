using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Dashboard;
public class TopDoctorMetric
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int TotalActivityCount { get; set; }
    public Dictionary<string, int> WeeklyActivityCounts { get; set; } = new();
}
