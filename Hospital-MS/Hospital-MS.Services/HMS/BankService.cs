using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Banks;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class BankService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IBankService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sQLHelper = sQLHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(BankRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var bank = new Bank
            {
                Name = request.Name,
                IsActive = true
            };
            await _unitOfWork.Repository<Bank>().AddAsync(bank, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, bank.Id.ToString());
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
            var bank = _unitOfWork.Repository<Bank>()
                .GetAll(x => x.Id == id)
                .FirstOrDefault();

            if (bank == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            bank.IsActive = false;

            _unitOfWork.Repository<Bank>().Update(bank);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, bank.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new SqlParameter[]
            {
            new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
            new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
            new SqlParameter("@PageSize", pagingFilter.PageSize)
            };

            var dt = await _sQLHelper.ExecuteDataTableAsync("[finance].[SP_GetAllBanks]", parameters);

            int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int?>("TotalCount") ?? 0 : 0;

            return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
        }
        catch (Exception)
        {
            return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<BankResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var bank = await _unitOfWork.Repository<Bank>()
                .GetAll(x => x.Id == id && x.IsActive)
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync();

            if (bank == null)
            {
                return ErrorResponseModel<BankResponse>.Failure(GenericErrors.NotFound);
            }

            var response = new BankResponse
            {
                Id = bank.Id,
                Name = bank.Name,
                IsActive = bank.IsActive,
                Audit = new Core.Contracts.Common.AuditResponse
                {
                    CreatedBy = bank.CreatedBy?.UserName,
                    CreatedOn = bank.CreatedOn,
                    UpdatedBy = bank.UpdatedBy?.UserName,
                    UpdatedOn = bank.UpdatedOn
                },
            };

            return ErrorResponseModel<BankResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<BankResponse>.Failure(GenericErrors.TransFailed);
        }

    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, BankRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var bank = _unitOfWork.Repository<Bank>()
                .GetAll(x => x.Id == id && x.IsActive)
                .FirstOrDefault();

            if (bank == null)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);
            }

            bank.Name = request.Name;

            _unitOfWork.Repository<Bank>().Update(bank);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, bank.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);

        }
    }
}