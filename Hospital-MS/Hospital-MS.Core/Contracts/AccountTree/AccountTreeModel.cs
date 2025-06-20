using Hospital_MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.AccountTree
{
    public class AccountTreeModel
    {
        public int? AccountId { get; set; }
        public int? CostCenterId { get; set; }
        public string? AccountNumber { get; set; }
        public int? ParentAccountId { get; set; }
        public int? AccountLevel { get; set; }
        public int? AccountTypeId { get; set; }
        public string? NameAR { get; set; }
        public string? NameEN { get; set; }
        public string? AssetType { get; set; }
        public string? DescriptionMethod { get; set; }
        public double? PreCredit { get; set; }
        public double? PreDebit { get; set; }
        public bool? IsSelected { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool? IsDisToCostCenter { get; set; }
        public bool? IsParent { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<AccountTreeModel> Children { get; set; } = new List<AccountTreeModel>();
    }
}
