using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models
{
    [Table("AccountTrees", Schema = "Finance")]
    public class AccountTree
    {
        [Key]
        public int AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public int? ParentAccountId { get; set; }
        public int? AccountLevel { get; set; }
        public int? AccountTypeId { get; set; }
        public string? NameAR { get; set; }
        public string? NameEN { get; set; }
        public bool? IsParent { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsReadOnly { get; set; }
        public int? CostCenterId { get; set; }
        public string? AccountNature { get; set; }
        public string? FName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsDisToCostCenter { get; set; }
        public bool? IsPost { get; set; }
        public string? AssetType { get; set; }
        public string? DepreciationMethod { get; set; }
        public int? DepreciationYears { get; set; }
        public int? DepreciationId { get; set; }
        public int? AccumulatedDepreciationId { get; set; }
        public double? PreCredit { get; set; }
        public double? PreDebit { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
