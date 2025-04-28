using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Patients
{
    public class PatientAttachmentRequest
    {
        //public int PatientId { get; set; }
        public IFormFile File { get; set; }
    }
}
