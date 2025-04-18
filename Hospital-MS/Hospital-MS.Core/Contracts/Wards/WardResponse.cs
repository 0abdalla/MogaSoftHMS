using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.Wards
{
    public class WardResponse
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
    }
}
