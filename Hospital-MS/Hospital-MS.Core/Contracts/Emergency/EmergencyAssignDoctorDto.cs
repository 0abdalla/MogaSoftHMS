using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Emergency;
public class EmergencyAssignDoctorDto
{
    public int EmergencyVisitId { get; set; }
    public int DoctorId { get; set; }
}
