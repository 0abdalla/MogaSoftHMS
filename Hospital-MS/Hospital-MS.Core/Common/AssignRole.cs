using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Common
{
    public class AssignRole
    {
        public Guid RoleId { get; set; }
        public List<int> PageIds { get; set; }
    }
}
