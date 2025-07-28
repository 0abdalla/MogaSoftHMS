using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.Disbursement;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Core.Models.HR;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class DisbursementRequestService(IUnitOfWork unitOfWork) : IDisbursementRequestService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> ApproveDisbursementRequestAsync(int id, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>().GetByIdAsync(id, cancellationToken);

            if (disbursementRequest == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            disbursementRequest.Status = PurchaseStatus.Approved;
            _unitOfWork.Repository<DisbursementRequest>().Update(disbursementRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<DisbursementToReturnResponse>> CreateAsync(DisbursementReq request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var department = await _unitOfWork.Repository<JobDepartment>()
                .GetByIdAsync(request.JobDepartmentId ?? 0, cancellationToken);

            if (department == null)
            {
                return ErrorResponseModel<DisbursementToReturnResponse>.Failure(GenericErrors.NotFound, null);
            }

            var disbursementRequest = new DisbursementRequest
            {
                Number = await GenerateDisbursementNumber(cancellationToken),
                Date = request.Date,
                Notes = request.Notes,
                JobDepartmentId = request.JobDepartmentId,
                Items = request.Items.Select(item => new DisbursementRequestItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                }).ToList()
            };
            await _unitOfWork.Repository<DisbursementRequest>().AddAsync(disbursementRequest, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            var items = await _unitOfWork.Repository<DisbursementRequest>()
                .GetAll(x => x.Id == disbursementRequest.Id)
                .Include(x => x.JobDepartment)
                .Include(x => x.Items).ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(cancellationToken);

            var response = new DisbursementToReturnResponse
            {
                Id = disbursementRequest.Id,
                Number = disbursementRequest.Number,
                DepartmentName = department?.Name,
                ItemsNames = items?.Items.Select(i => i.Item?.NameAr ?? i.Item?.NameEn ?? "").ToList()
            };
            return ErrorResponseModel<DisbursementToReturnResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<DisbursementToReturnResponse>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>().GetByIdAsync(id, cancellationToken);

            if (disbursementRequest == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            disbursementRequest.IsActive = false;

            _unitOfWork.Repository<DisbursementRequest>().Update(disbursementRequest);

            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<DisbursementResponse>>> GetAllApprovedAsync(CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<DisbursementRequest>()
            .GetAll()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.JobDepartment)
            .Include(x => x.Items)
                .ThenInclude(i => i.Item)
                .ThenInclude(x => x.Unit)
            .Where(x => x.IsActive && x.Status == PurchaseStatus.Approved);

        var totalCount = await query.CountAsync(cancellationToken);
        var disbursementRequests = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        var responses = disbursementRequests.Select(disbursementRequest => new DisbursementResponse
        {
            Id = disbursementRequest.Id,
            Number = disbursementRequest.Number,
            Date = disbursementRequest.Date,
            Notes = disbursementRequest.Notes,
            JobDepartmentId = disbursementRequest.JobDepartmentId,
            JobDepartmentName = disbursementRequest.JobDepartment?.Name,
            Status = disbursementRequest.Status.ToString(),
            Items = disbursementRequest.Items.Select(item => new DisbursementItemResponse
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                ItemName = item.Item?.NameAr ?? item.Item?.NameEn ?? "",
                Unit = item.Item?.Unit.Name,
                Price = item.Item.Price,
                PriceAfterTax = item.Item.PriceAfterTax
            }).ToList()
        }).ToList();

        return PagedResponseModel<List<DisbursementResponse>>.Success(GenericErrors.GetSuccess, totalCount, responses);
    }

    public async Task<PagedResponseModel<List<DisbursementResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        var query = _unitOfWork.Repository<DisbursementRequest>()
            .GetAll()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.JobDepartment)
            .Include(x => x.Items)
                .ThenInclude(i => i.Item)
                .ThenInclude(x => x.Unit)
            .Where(x => x.IsActive);

        if (!string.IsNullOrEmpty(filter.SearchText))
        {
            query = query.Where(x => x.Notes.Contains(filter.SearchText) ||
                                     x.JobDepartment.Name.Contains(filter.SearchText));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var disbursementRequests = await query
            .OrderByDescending(x => x.Id)
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        var responses = disbursementRequests.Select(disbursementRequest => new DisbursementResponse
        {
            Id = disbursementRequest.Id,
            Number = disbursementRequest.Number,
            Date = disbursementRequest.Date,
            Notes = disbursementRequest.Notes,
            JobDepartmentId = disbursementRequest.JobDepartmentId,
            JobDepartmentName = disbursementRequest.JobDepartment?.Name,
            Status = disbursementRequest.Status.ToString(),
            Items = disbursementRequest.Items.Select(item => new DisbursementItemResponse
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                ItemName = item.Item?.NameAr ?? item.Item?.NameEn ?? "",
                Unit = item.Item?.Unit?.Name,
                Price = item.Item.Price,
                PriceAfterTax = item.Item.PriceAfterTax
            }).ToList()
        }).ToList();

        return PagedResponseModel<List<DisbursementResponse>>.Success(GenericErrors.GetSuccess, totalCount, responses);

    }

    public async Task<ErrorResponseModel<DisbursementResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>()
                                                .GetAll(x => x.Id == id)
                                                .Include(x => x.CreatedBy)
                                                .Include(x => x.UpdatedBy)
                                                .Include(x => x.JobDepartment)
                                                .Include(x => x.Items)
                                                    .ThenInclude(i => i.Item)
                                                    .ThenInclude(x => x.Unit)
                                                .FirstOrDefaultAsync(cancellationToken);



        if (disbursementRequest == null)
        {
            return ErrorResponseModel<DisbursementResponse>.Failure(GenericErrors.NotFound);
        }

        var response = new DisbursementResponse
        {
            Id = disbursementRequest.Id,
            Number = disbursementRequest.Number,
            Date = disbursementRequest.Date,
            Notes = disbursementRequest.Notes,
            JobDepartmentId = disbursementRequest.JobDepartmentId,
            JobDepartmentName = disbursementRequest.JobDepartment?.Name,
            Status = disbursementRequest.Status.ToString(),
            Items = disbursementRequest.Items.Select(item => new DisbursementItemResponse
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
                ItemName = item.Item?.NameAr ?? item.Item?.NameEn ?? "",
                Unit = item.Item?.Unit?.Name,
                Price = item.Item.Price,
                PriceAfterTax = item.Item.PriceAfterTax
            }).ToList(),
            Audit = new AuditResponse
            {
                CreatedBy = disbursementRequest.CreatedBy?.UserName ?? "",
                CreatedOn = disbursementRequest.CreatedOn,
                UpdatedBy = disbursementRequest.UpdatedBy?.UserName,
                UpdatedOn = disbursementRequest.UpdatedOn
            },
        };

        return ErrorResponseModel<DisbursementResponse>.Success(GenericErrors.GetSuccess, response);
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, DisbursementReq request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var disbursementRequest = await _unitOfWork.Repository<DisbursementRequest>().GetByIdAsync(id, cancellationToken);
            if (disbursementRequest == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            disbursementRequest.Date = request.Date;
            disbursementRequest.Notes = request.Notes;
            disbursementRequest.JobDepartmentId = request.JobDepartmentId;

            // Clear existing items and add new ones
            disbursementRequest.Items.Clear();
            disbursementRequest.Items = request.Items.Select(item => new DisbursementRequestItem
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity,
            }).ToList();

            // Update the disbursement request

            _unitOfWork.Repository<DisbursementRequest>().Update(disbursementRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.GetSuccess);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }

    }

    private async Task<string> GenerateDisbursementNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<DisbursementRequest>()
            .CountAsync(x => x.CreatedOn.Year == year, cancellationToken);
        return $"DR-{year}-{(count + 1):D5}";
    }
}
