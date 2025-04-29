using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Rooms
{
    public class RoomResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal DailyPrice { get; set; }

        public int WardId { get; set; }
        public int WardNumber { get; set; } 
    }
}
