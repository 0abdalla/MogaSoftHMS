using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Beds
{
    public class CreateBedRequest
    {
        public int Number { get; set; } 
        public string Status { get; set; }
        public int RoomId { get; set; }
    }
}
