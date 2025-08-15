using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_MS.Core.Models
{
    [Table("Pages", Schema = "config")]
    public class Page
    {
        public int Id { get; set; }
        public string NameAR { get; set; }
        public string? PageName { get; set; }
    }
}
