using Hospital_MS.Core.Models;
using Hospital_MS.Core.Wrappers;
using Microsoft.EntityFrameworkCore;
using mogaERP.Domain.Specifications;

namespace Hospital_MS.Services.Specifications;
public class ItemSpecification : BaseSpecification<Item>
{
    public ItemSpecification(SearchRequest request)
        : base(i => true)

    {
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            AddCriteria(i => i.NameAR.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (request.SortDescending)
                ApplyOrderByDescending(e => EF.Property<object>(e, request.SortBy));
            else
                ApplyOrderBy(e => EF.Property<object>(e, request.SortBy));
        }

        ApplyPagination((request.PageNumber - 1) * request.PageSize, request.PageSize);


        AddIncludes();
    }

    public ItemSpecification(int id)
        : base(i => i.Id == id && i.IsDeleted == false)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        AddInclude(i => i.Unit);
        AddInclude(i => i.Group);
        AddInclude(i => i.CreatedBy);
        AddInclude(i => i.UpdatedBy);
        AddInclude("Group.MainGroup");
    }


    //public class ItemStoreBalanceSpecification : BaseSpecification<ItemBatch>
    //{
    //    public ItemStoreBalanceSpecification(int itemId, int storeId)
    //        : base(x => x.ItemId == itemId && x.IsDeleted == false
    //                 && x.ReceiptPermission != null
    //                 && x.ReceiptPermission.StoreId == storeId)
    //    {
    //        AddInclude(x => x.ReceiptPermission);
    //    }
    //}
}


