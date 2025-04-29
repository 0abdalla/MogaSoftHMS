using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Rooms
{
    public class CreateRoomRequest
    {
        public int Number { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal DailyPrice { get; set; }
        public int WardId { get; set; }
    }
}
