using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    [Table("Pages", Schema = "config")]
    public class Page
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string? PageName { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string? Route { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public List<Page> Children { get; set; } = new List<Page>();
    }
}
