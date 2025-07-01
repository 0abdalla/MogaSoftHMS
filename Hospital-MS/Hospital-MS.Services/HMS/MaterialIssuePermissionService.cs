using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MaterialIssuePermission;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class MaterialIssuePermissionService(IUnitOfWork unitOfWork) : IMaterialIssuePermissionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var store = await _unitOfWork.Repository<Store>().GetByIdAsync(request.StoreId, cancellationToken);
            if (store is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound,"المخزن غير موجود");

            var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(request.BranchId, cancellationToken);
            if (branch is null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound,"الفرع غير موجود");

            var permission = new MaterialIssuePermission
            {
                PermissionNumber = await GeneratePermissionNumber(cancellationToken),
                DocumentNumber = request.DocumentNumber,
                PermissionDate = DateOnly.FromDateTime(request.PermissionDate),
                StoreId = request.StoreId,
                BranchId = request.BranchId,
                Notes = request.Notes,
                IsActive = true,
                Items = request.Items.Select(i => new MaterialIssueItem
                {
                    ItemId = i.ItemId,
                    Unit = i.Unit,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    IsActive = true
                }).ToList()
            };

            await _unitOfWork.Repository<MaterialIssuePermission>().AddAsync(permission, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, permission.Id.ToString());
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, MaterialIssuePermissionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var permission = await _unitOfWork.Repository<MaterialIssuePermission>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (permission == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            permission.DocumentNumber = request.DocumentNumber;
            permission.PermissionDate = DateOnly.FromDateTime(request.PermissionDate);
            permission.StoreId = request.StoreId;
            permission.BranchId = request.BranchId;
            permission.Notes = request.Notes;

            foreach (var item in permission.Items)
                item.IsActive = false;

            permission.Items = request.Items.Select(i => new MaterialIssueItem
            {
                ItemId = i.ItemId,
                Unit = i.Unit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                IsActive = true
            }).ToList();

            _unitOfWork.Repository<MaterialIssuePermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, permission.Id.ToString());
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<MaterialIssuePermission>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (permission == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            permission.IsActive = false;
            foreach (var item in permission.Items)
                item.IsActive = false;

            _unitOfWork.Repository<MaterialIssuePermission>().Update(permission);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, permission.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<MaterialIssuePermissionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var permission = await _unitOfWork.Repository<MaterialIssuePermission>()
                .GetAll()
                .Include(x => x.Store)
                .Include(x => x.Branch)
                .Include(x => x.Items)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (permission == null)
                return ErrorResponseModel<MaterialIssuePermissionResponse>.Failure(GenericErrors.NotFound);

            var response = new MaterialIssuePermissionResponse
            {
                Id = permission.Id,
                PermissionNumber = permission.PermissionNumber,
                DocumentNumber = permission.DocumentNumber,
                PermissionDate = permission.PermissionDate,
                StoreId = permission.StoreId,
                StoreName = permission.Store.Name,
                BranchId = permission.BranchId,
                BranchName = permission.Branch.Name,
                Notes = permission.Notes,
                Items = permission.Items.Where(i => i.IsActive).Select(i => new MaterialIssueItemResponse
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item.NameAr,
                    Unit = i.Unit,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            return ErrorResponseModel<MaterialIssuePermissionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch
        {
            return ErrorResponseModel<MaterialIssuePermissionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<MaterialIssuePermissionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<MaterialIssuePermission>()
                .GetAll()
                .Include(x => x.Store)
                .Include(x => x.Branch)
                .Include(x => x.Items)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x =>
                    x.PermissionNumber.Contains(filter.SearchText) ||
                    x.Notes.Contains(filter.SearchText));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new MaterialIssuePermissionResponse
                {
                    Id = x.Id,
                    PermissionNumber = x.PermissionNumber,
                    DocumentNumber = x.DocumentNumber,
                    PermissionDate = x.PermissionDate,
                    StoreId = x.StoreId,
                    StoreName = x.Store.Name,
                    BranchId = x.BranchId,
                    BranchName = x.Branch.Name,
                    Notes = x.Notes,
                    Items = x.Items.Where(i => i.IsActive).Select(i => new MaterialIssueItemResponse
                    {
                        ItemId = i.ItemId,
                        ItemName = i.Item.NameAr,
                        Unit = i.Unit,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.TotalPrice
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<MaterialIssuePermissionResponse>>.Success(GenericErrors.GetSuccess, totalCount, list);
        }
        catch
        {
            return PagedResponseModel<List<MaterialIssuePermissionResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    private async Task<string> GeneratePermissionNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<MaterialIssuePermission>()
            .CountAsync(x => x.PermissionDate.Year == year, cancellationToken);
        return $"MI-{year}-{(count + 1):D5}";
    }
}
