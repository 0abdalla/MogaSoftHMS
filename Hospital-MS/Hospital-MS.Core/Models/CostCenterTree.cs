using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    [Table("CostCenterTree", Schema = "Finance")]
    public class CostCenterTree
    {
        public int Id { get; set; }
        public string CostCenterNumber { get; set; } = string.Empty;
        public string NameAR { get; set; } = string.Empty;
        public string? NameEN { get; set; }
        public int? ParentCostCenterId { get; set; }
        public CostCenterTree? ParentCostCenter { get; set; }
        public ICollection<CostCenterTree> Children { get; set; } = new List<CostCenterTree>();
        public int CostLevel { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsParent { get; set; } = false;
        public bool AllowPosting { get; set; } = true;
        public bool IsExpenses { get; set; } = false;
        public int? DisplayOrder { get; set; }
        public ICollection<AccountTree>? LinkedAccounts { get; set; } = new List<AccountTree>();
        public bool IsDeleted { get; set; } = false;

        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedById { get; set; }

        public ApplicationUser? CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
