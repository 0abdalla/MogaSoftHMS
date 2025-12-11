using Hospital_MS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;

    public class AccountTree
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public int? ParentAccountId { get; set; }
        public AccountTree? ParentAccount { get; set; }
        public ICollection<AccountTree> Children { get; set; } = new List<AccountTree>();
        public int AccountLevel { get; set; }
        public AccountTypes AccountType { get; set; }
        //public AccountType? AccountType { get; set; }
        public string NameAR { get; set; } = string.Empty;
        public string? NameEN { get; set; }
        public bool IsParent { get; set; } = false;
        public bool AllowPosting { get; set; } = true; // الحساب ده تفصيلي ولا تجميعي يعني يسمح بترحيل القيود المحاسبية ولا لا
        public AccountNature AccountNature { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CostCenterId { get; set; }
        public CostCenterTree? CostCenterTree { get; set; }
        public Double? PreDebit { get; set; }
        public Double? PreCredit { get; set; }

        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedById { get; set; }

        public ApplicationUser? CreatedBy { get; set; } = default!;
        public ApplicationUser? UpdatedBy { get; set; }

        // no need for AccountId or audit fields because they're inherited from BaseEntity
        // no need for IsGroup or IsReadOnly
        // no need for IsLocked because of IsActive
        // no need for IsDisToCostCenter because of CostCenterId
        // no need for IsPost because of AllowPosting
        // no need for AssetType
        // no need for AccumulatedDepreciationId and the same of it because it should be in another table called (FixedAssets)
    }


