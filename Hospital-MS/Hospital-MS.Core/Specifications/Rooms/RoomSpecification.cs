using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Specifications.Rooms
{
    public class RoomSpecification : BaseSpecification<Room>
    {
        public RoomSpecification()
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(r => r.Ward);
        }
    }
}
