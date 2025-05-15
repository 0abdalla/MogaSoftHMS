using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    [Table("RolePermissions", Schema = "config")]
    public class RolePermission
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public Guid RoleId { get; set; }
    }
}
