using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Attendance;
public class CreateAttendanceRequest
{
    public IFormFile File { get; set; }
}
