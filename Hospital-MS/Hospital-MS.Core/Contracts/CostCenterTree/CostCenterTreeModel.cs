using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.CostCenterTree
{
    public class CostCenterTreeModel
    {
        public int? CostCenterId { get; set; }
        public string CostCenterNumber { get; set; }
        public string NameAR { get; set; }
        public string? NameEN { get; set; }
        public int? ParentId { get; set; }
        public int? CostLevel { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsParent { get; set; }
        public bool? IsPost { get; set; }
        public int? IsExpences { get; set; }
        public bool? IsGroup { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsSelected { get; set; }
        public List<CostCenterTreeModel> Children { get; set; } = new List<CostCenterTreeModel>();
    }
}
