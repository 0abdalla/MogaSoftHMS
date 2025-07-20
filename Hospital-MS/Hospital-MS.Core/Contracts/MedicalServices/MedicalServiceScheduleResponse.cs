using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.MedicalServices
{
    public class MedicalServiceScheduleResponse
    {
        public int Id { get; set; }
        public string WeekDay { get; set; }
    }

    public class RadiologyBodyTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
