using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PriceQuotation;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class PriceQuotationService : IPriceQuotationService
{
    private readonly IUnitOfWork _unitOfWork;

    public PriceQuotationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorResponseModel<string>> CreateAsync(PriceQuotationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var purchase = await _unitOfWork.Repository<PurchaseRequest>()
                .GetAll(x => x.Id == request.PurchaseRequestId)
                .FirstOrDefaultAsync(cancellationToken);

            if (purchase == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound, "طلب الشراء غير موجود");
            else if (purchase != null && purchase.Status != PurchaseStatus.Approved)
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed, "طلب الشراء غير مأكد");

            var quotation = new PriceQuotation
            {
                QuotationNumber = await GenerateQuotationNumber(cancellationToken),
                QuotationDate = request.QuotationDate,
                SupplierId = request.SupplierId,
                Notes = request.Notes,
                Status = QuotationStatus.Pending,
                PurchaseRequestId = request.PurchaseRequestId,
                Items = request.Items.Select(i => new PriceQuotationItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Notes = i.Notes,
                    IsActive = true
                }).ToList()
            };

            await _unitOfWork.Repository<PriceQuotation>().AddAsync(quotation, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, quotation.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetAll(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            if (quotation == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }
            quotation.IsActive = false;
            _unitOfWork.Repository<PriceQuotation>().Update(quotation);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<PriceQuotationResponse>>> GetAllApprovedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<PriceQuotation>()
                  .GetAll()
                  .Include(x => x.Supplier)
                  .Include(x => x.Items)
                  .Include(x => x.PurchaseRequest)
                  .Where(x => x.IsActive && x.Status == QuotationStatus.Approved);

            var totalCount = await query.CountAsync(cancellationToken);

            var quotations = await query
                  .OrderByDescending(x => x.QuotationDate)
                  .Select(x => new PriceQuotationResponse
                  {
                      Id = x.Id,
                      QuotationNumber = x.QuotationNumber,
                      QuotationDate = x.QuotationDate,
                      SupplierId = x.SupplierId,
                      SupplierName = x.Supplier.Name,
                      Notes = x.Notes,
                      Status = x.Status.ToString(),
                      TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                      PurchaseRequestId = x.PurchaseRequestId,
                      PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                      Items = x.Items.Where(i => i.IsActive).Select(i => new PriceQuotationItemResponse
                      {
                          Id = i.Id,
                          NameAr = i.Item.NameAr,
                          Quantity = i.Quantity,
                          UnitPrice = i.UnitPrice,
                          Total = i.Quantity * i.UnitPrice,
                          Notes = i.Notes
                      }).ToList()
                  })
                  .ToListAsync(cancellationToken);

            return PagedResponseModel<List<PriceQuotationResponse>>.Success(GenericErrors.GetSuccess, totalCount, quotations);

        }
        catch (Exception)
        {
            return PagedResponseModel<List<PriceQuotationResponse>>.Failure(GenericErrors.TransFailed); throw;
        }
    }

    public async Task<PagedResponseModel<List<PriceQuotationResponse>>> GetAllAsync(
        PagingFilterModel filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<PriceQuotation>()
                .GetAll()
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .Include(x => x.PurchaseRequest)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x =>
                    x.QuotationNumber.Contains(filter.SearchText) ||
                    x.Supplier.Name.Contains(filter.SearchText));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var quotations = await query
                .OrderByDescending(x => x.QuotationDate)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PriceQuotationResponse
                {
                    Id = x.Id,
                    QuotationNumber = x.QuotationNumber,
                    QuotationDate = x.QuotationDate,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    PurchaseRequestId = x.PurchaseRequestId,
                    PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                    Items = x.Items.Where(i => i.IsActive).Select(i => new PriceQuotationItemResponse
                    {
                        Id = i.Id,
                        NameAr = i.Item.NameAr,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Total = i.Quantity * i.UnitPrice,
                        Notes = i.Notes
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<PriceQuotationResponse>>.Success(
                GenericErrors.GetSuccess,
                totalCount,
                quotations);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<PriceQuotationResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<PriceQuotationResponse>>> GetAllByPurchaseRequestIdAsync(int purchaseRequestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<PriceQuotation>()
                .GetAll()
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .ThenInclude(i => i.Item).ThenInclude(i => i.Unit)
                .Include(x => x.PurchaseRequest)
                .Where(x => x.IsActive && x.PurchaseRequestId == purchaseRequestId);

            var totalCount = await query.CountAsync(cancellationToken);

            var quotations = await query
                .OrderByDescending(x => x.QuotationDate)
                .Select(x => new PriceQuotationResponse
                {
                    Id = x.Id,
                    QuotationNumber = x.QuotationNumber,
                    QuotationDate = x.QuotationDate,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    PurchaseRequestId = x.PurchaseRequestId,
                    PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                    Items = x.Items.Where(i => i.IsActive).Select(i => new PriceQuotationItemResponse
                    {
                        Id = i.Id,
                        NameAr = i.Item.NameAr,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Total = i.Quantity * i.UnitPrice,
                        Notes = i.Notes,
                        Unit = i.Item.Unit.Name
                    }).ToList()

                }).ToListAsync(cancellationToken);


            return PagedResponseModel<List<PriceQuotationResponse>>.Success(GenericErrors.GetSuccess, totalCount, quotations);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<PriceQuotationResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<PriceQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotation = await _unitOfWork.Repository<PriceQuotation>()
                .GetAll()
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .Include(x => x.PurchaseRequest)
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new PriceQuotationResponse
                {
                    Id = x.Id,
                    QuotationNumber = x.QuotationNumber,
                    QuotationDate = x.QuotationDate,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    PurchaseRequestId = x.PurchaseRequestId,
                    PurchaseRequestNumber = x.PurchaseRequest.RequestNumber,
                    Items = x.Items.Where(i => i.IsActive).Select(i => new PriceQuotationItemResponse
                    {
                        Id = i.ItemId,
                        NameAr = i.Item.NameAr,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Total = i.Quantity * i.UnitPrice,
                        Notes = i.Notes
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (quotation == null)
            {
                return ErrorResponseModel<PriceQuotationResponse>.Failure(GenericErrors.NotFound);
            }
            return ErrorResponseModel<PriceQuotationResponse>.Success(GenericErrors.GetSuccess, quotation);
        }
        catch (Exception)
        {
            return ErrorResponseModel<PriceQuotationResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<string>> SubmitPriceQuotationByPurchaseRequestIdAsync(int purchaseRequestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotations = await _unitOfWork.Repository<PriceQuotation>()
                .GetAll()
                .Include(x => x.Items)
                .Where(x => x.IsActive && x.PurchaseRequestId == purchaseRequestId)
                .ToListAsync(cancellationToken);

            if (quotations.Count == 0)
                return PagedResponseModel<string>.Failure(GenericErrors.NotFound);


            var quotationsWithTotal = quotations
                .Select(q => new
                {
                    Quotation = q,
                    TotalAmount = q.Items.Where(i => i.IsActive).Sum(i => i.Quantity * i.UnitPrice)
                })
                .ToList();

            var minQuotation = quotationsWithTotal
                .OrderBy(q => q.TotalAmount)
                .First();

            foreach (var q in quotationsWithTotal)
            {
                q.Quotation.Status = q == minQuotation ? QuotationStatus.Approved : QuotationStatus.Rejected;
                _unitOfWork.Repository<PriceQuotation>().Update(q.Quotation);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            return PagedResponseModel<string>.Success(GenericErrors.GetSuccess, 1, "تم التحديث بنجاح");
        }
        catch (Exception)
        {
            return PagedResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<string>> SubmitPriceQuotationByPurchaseRequestIdAsyncV2(int purchaseRequestId, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotations = await _unitOfWork.Repository<PriceQuotation>()
                .GetAll()
                .Include(x => x.Items)
                .Where(x => x.IsActive && x.PurchaseRequestId == purchaseRequestId)
                .ToListAsync(cancellationToken);

            if (quotations.Count == 0)
                return PagedResponseModel<string>.Failure(GenericErrors.NotFound);

            var quotationsWithTotal = quotations
                .Select(q => new
                {
                    Quotation = q,
                    TotalAmount = q.Items.Where(i => i.IsActive).Sum(i => i.Quantity * i.UnitPrice)
                })
                .ToList();

            var minQuotation = quotationsWithTotal
                .OrderBy(q => q.TotalAmount)
                .First();

            foreach (var q in quotationsWithTotal)
            {
                q.Quotation.Status = q == minQuotation ? QuotationStatus.Approved : QuotationStatus.Rejected;
                _unitOfWork.Repository<PriceQuotation>().Update(q.Quotation);
            }

            // Update PurchaseRequest with selected PriceQuotationId
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .GetAll()
                .FirstOrDefaultAsync(x => x.Id == purchaseRequestId, cancellationToken);

            if (purchaseRequest != null)
            {
                purchaseRequest.PriceQuotationId = minQuotation.Quotation.Id;
                _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            return PagedResponseModel<string>.Success(GenericErrors.GetSuccess, 1, "تم التحديث بنجاح");
        }
        catch (Exception)
        {
            return PagedResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, PriceQuotationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetByIdAsync(id, cancellationToken);
            if (quotation == null || !quotation.IsActive)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            var purchase = await _unitOfWork.Repository<PurchaseOrder>()
                .GetAll(x => x.Id == request.PurchaseRequestId)
                .FirstOrDefaultAsync(cancellationToken);

            if (purchase == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound, "طلب الشراء غير موجود");
            else if (purchase != null && purchase.Status != PurchaseStatus.Approved)
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed, "طلب الشراء غير مأكد");

            quotation.QuotationDate = request.QuotationDate;
            quotation.SupplierId = request.SupplierId;
            quotation.Notes = request.Notes;
            quotation.PurchaseRequestId = request.PurchaseRequestId;

            // Clear existing items and add new ones
            quotation.Items.Clear();
            foreach (var item in request.Items)
            {
                quotation.Items.Add(new PriceQuotationItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Notes = item.Notes,
                    IsActive = true
                });
            }
            _unitOfWork.Repository<PriceQuotation>().Update(quotation);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    private async Task<string> GenerateQuotationNumber(CancellationToken cancellationToken)
    {
        var year = DateTime.Now.Year;
        var count = await _unitOfWork.Repository<PriceQuotation>()
            .CountAsync(x => x.QuotationDate.Year == year, cancellationToken);
        return $"PQ-{year}-{(count + 1):D5}";
    }
}