using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Staff
{
    public class StaffCountsResponse
    {
        public int DoctorsCount { get; set; }
        public int NursesCount { get; set; }
        public int AdministratorsCount { get; set; }
        public int WorkersCount { get; set; }
    }
}
