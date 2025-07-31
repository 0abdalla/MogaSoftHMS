using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.DailyRestrictions;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.HMS;

public class DailyRestrictionService(IUnitOfWork unitOfWork) : IDailyRestrictionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorResponseModel<string>> CreateAsync(DailyRestrictionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var entity = new DailyRestriction
            {
                RestrictionNumber = await GenerateRestrictionNumberAsync(cancellationToken),
                RestrictionDate = request.RestrictionDate,
                RestrictionTypeId = request.RestrictionTypeId,
                //LedgerNumber = request.LedgerNumber,
                Description = request.Description,
                AccountingGuidanceId = request.AccountingGuidanceId,
                IsActive = true,
                Details = request.Details.Select(d => new DailyRestrictionDetail
                {
                    AccountId = d.AccountId,
                    Debit = d.Debit,
                    Credit = d.Credit,
                    CostCenterId = d.CostCenterId,
                    Note = d.Note
                }).ToList()
            };

            await _unitOfWork.Repository<DailyRestriction>().AddAsync(entity, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, entity.Id.ToString());
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, DailyRestrictionRequest request, CancellationToken cancellationToken = default)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var entity = await _unitOfWork.Repository<DailyRestriction>()
                .GetAll()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            if (string.IsNullOrWhiteSpace(entity.RestrictionNumber))
            {
                entity.RestrictionNumber = await GenerateRestrictionNumberAsync(cancellationToken);
            }
            entity.RestrictionDate = request.RestrictionDate;
            entity.RestrictionTypeId = request.RestrictionTypeId;
            //entity.LedgerNumber = request.LedgerNumber;
            entity.Description = request.Description;
            entity.AccountingGuidanceId = request.AccountingGuidanceId;

            entity.Details.Clear();
            foreach (var d in request.Details)
            {
                entity.Details.Add(new DailyRestrictionDetail
                {
                    AccountId = d.AccountId,
                    Debit = d.Debit,
                    Credit = d.Credit,
                    CostCenterId = d.CostCenterId,
                    Note = d.Note
                });
            }

            _unitOfWork.Repository<DailyRestriction>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, entity.Id.ToString());
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
            var entity = await _unitOfWork.Repository<DailyRestriction>()
                .GetAll()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (entity == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            entity.IsActive = false;
            _unitOfWork.Repository<DailyRestriction>().Update(entity);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, entity.Id.ToString());
        }
        catch
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<DailyRestrictionResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _unitOfWork.Repository<DailyRestriction>()
                .GetAll()
                .Include(x => x.RestrictionType)
                .Include(x => x.Details)
                    .ThenInclude(d => d.Account)
                .Include(x => x.Details)
                    .ThenInclude(d => d.CostCenter)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Include(x => x.AccountingGuidance)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (entity == null)
                return ErrorResponseModel<DailyRestrictionResponse>.Failure(GenericErrors.NotFound);

            var response = new DailyRestrictionResponse
            {
                Id = entity.Id,
                RestrictionNumber = entity.RestrictionNumber,
                RestrictionDate = entity.RestrictionDate,
                RestrictionTypeId = entity.RestrictionTypeId,
                RestrictionTypeName = entity.RestrictionType.Name,
                //LedgerNumber = entity.LedgerNumber,
                Description = entity.Description,
                Details = entity.Details.Select(d => new DailyRestrictionDetailResponse
                {
                    Id = d.Id,
                    AccountId = d.AccountId,
                    AccountName = d.Account.NameAR ?? d.Account.NameEN ?? "",
                    Debit = d.Debit,
                    Credit = d.Credit,
                    CostCenterId = d.CostCenterId,
                    CostCenterName = d.CostCenter?.NameAR,
                    Note = d.Note
                }).ToList(),

                AccountingGuidanceId = entity.AccountingGuidanceId,
                AccountingGuidanceName = entity.AccountingGuidance?.Name ?? "",
                Audit = new AuditResponse
                {
                    CreatedBy = entity.CreatedBy != null ? entity.CreatedBy.UserName : null,
                    CreatedOn = entity.CreatedOn,
                    UpdatedBy = entity.UpdatedBy != null ? entity.UpdatedBy.UserName : null,
                    UpdatedOn = entity.UpdatedOn
                }

            };

            return ErrorResponseModel<DailyRestrictionResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch
        {
            return ErrorResponseModel<DailyRestrictionResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<DailyRestrictionResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _unitOfWork.Repository<DailyRestriction>()
                .GetAll()
                .Include(x => x.RestrictionType)
                .Include(x => x.Details)
                    .ThenInclude(x => x.CostCenter)
                .Include(x => x.Details)
                    .ThenInclude(x => x.Account)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                query = query.Where(x =>
                    x.RestrictionNumber.Contains(filter.SearchText) ||
                    x.Description.Contains(filter.SearchText));
            }

            var total = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.RestrictionDate)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new DailyRestrictionResponse
                {
                    Id = x.Id,
                    RestrictionNumber = x.RestrictionNumber,
                    RestrictionDate = x.RestrictionDate,
                    RestrictionTypeId = x.RestrictionTypeId,
                    RestrictionTypeName = x.RestrictionType.Name,
                    //LedgerNumber = x.LedgerNumber,
                    Description = x.Description,
                    AccountingGuidanceId = x.AccountingGuidanceId,
                    AccountingGuidanceName = x.AccountingGuidance != null ? x.AccountingGuidance.Name : null,

                    Details = x.Details.Select(d => new DailyRestrictionDetailResponse
                    {
                        Id = d.Id,
                        AccountId = d.AccountId,
                        AccountName = d.Account.NameAR ?? d.Account.NameEN ?? "",
                        Debit = d.Debit,
                        Credit = d.Credit,
                        CostCenterId = d.CostCenterId,
                        CostCenterName = d.CostCenter.NameAR,
                        Note = d.Note
                    }).ToList(),
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<DailyRestrictionResponse>>.Success(GenericErrors.GetSuccess, total, list);
        }
        catch
        {
            return PagedResponseModel<List<DailyRestrictionResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<string> GenerateRestrictionNumberAsync(CancellationToken cancellationToken = default)
    {
        var lastRestriction = await _unitOfWork.Repository<DailyRestriction>()
            .GetAll()
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;

        if (lastRestriction != null)
        {
            if (int.TryParse(lastRestriction.RestrictionNumber, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }
        return nextNumber.ToString("D5");
    }
}