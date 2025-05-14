using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Dashboard;
public class WeeklyTopDoctorMetrics
{
    //public string WeekName { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<TopDoctorMetric> TopDoctors { get; set; } = new();
}