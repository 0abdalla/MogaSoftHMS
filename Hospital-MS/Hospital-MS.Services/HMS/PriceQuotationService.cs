using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PriceQuotation;
using Hospital_MS.Core.Enums;
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
            var quotation = new PriceQuotation
            {
                QuotationNumber = await GenerateQuotationNumber(cancellationToken),
                QuotationDate = request.QuotationDate,
                SupplierId = request.SupplierId,
                Notes = request.Notes,
                Status = QuotationStatus.Pending,
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
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetAll(x => x.Id == id).FirstOrDefaultAsync();
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
                    SupplierName = x.Supplier.Name,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    Items = x.Items.Where(i => i.IsActive).Select(i => new PriceQuotationItemResponse
                    {
                        Id = i.Id,
                        ItemName = i.Item.NameAr,
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

    public async Task<ErrorResponseModel<PriceQuotationResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotation = await _unitOfWork.Repository<PriceQuotation>()
                .GetAll()
                .Include(x => x.Supplier)
                .Include(x => x.Items)
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new PriceQuotationResponse
                {
                    Id = x.Id,
                    QuotationNumber = x.QuotationNumber,
                    QuotationDate = x.QuotationDate,
                    SupplierName = x.Supplier.Name,
                    Notes = x.Notes,
                    Status = x.Status.ToString(),
                    TotalAmount = x.Items.Sum(i => i.Quantity * i.UnitPrice),
                    Items = x.Items.Where(i => i.IsActive).Select(i => new PriceQuotationItemResponse
                    {
                        Id = i.Id,
                        ItemName = i.Item.NameAr,
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

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, PriceQuotationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetByIdAsync(id, cancellationToken);
            if (quotation == null || !quotation.IsActive)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }
            quotation.QuotationDate = request.QuotationDate;
            quotation.SupplierId = request.SupplierId;
            quotation.Notes = request.Notes;
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