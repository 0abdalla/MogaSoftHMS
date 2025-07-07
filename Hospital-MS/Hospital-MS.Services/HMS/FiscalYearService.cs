using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.FiscalYears;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;
public class FiscalYearService(IUnitOfWork unitOfWork) : IFiscalYearService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(FiscalYearRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var overlap = await _unitOfWork.Repository<FiscalYear>()
                .GetAll(x => x.IsActive &&
                    (x.StartDate <= request.EndDate && x.EndDate >= request.StartDate))
                .AnyAsync(cancellationToken);

            if (overlap)
                return ErrorResponseModel<string>.Failure(new Error("يوجد سنة مالية متداخلة مع الفترة المحددة.", Status.Conflict));

            var entity = new FiscalYear
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            };

            await _unitOfWork.Repository<FiscalYear>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<FiscalYearResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<FiscalYear>()
                .GetAll()
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                if (DateOnly.TryParse(filter.SearchText, out var date))
                    query = query.Where(x => x.StartDate == date || x.EndDate == date);
            }

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.StartDate)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new FiscalYearResponse
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<FiscalYearResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch
        {
            return PagedResponseModel<List<FiscalYearResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<FiscalYearResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<FiscalYear>()
                .GetAll()
                .Where(x => x.Id == id && x.IsActive)
                .Select(x => new FiscalYearResponse
                {
                    Id = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Audit = new AuditResponse
                    {
                        CreatedBy = x.CreatedBy.UserName,
                        CreatedOn = x.CreatedOn,
                        UpdatedBy = x.UpdatedBy.UserName == null ? "" : x.UpdatedBy.UserName,
                        UpdatedOn = x.UpdatedOn
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return ErrorResponseModel<FiscalYearResponse>.Failure(GenericErrors.NotFound);

            return ErrorResponseModel<FiscalYearResponse>.Success(GenericErrors.GetSuccess, entity);
        }
        catch
        {
            return ErrorResponseModel<FiscalYearResponse>.Failure(GenericErrors.TransFailed);
        }
    }
}
