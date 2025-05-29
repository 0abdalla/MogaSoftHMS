using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.Suppliers;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services;
public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISQLHelper _sqlHelper;

    public SupplierService(IUnitOfWork unitOfWork, ISQLHelper sqlHelper)
    {
        _unitOfWork = unitOfWork;
        _sqlHelper = sqlHelper;
    }

    public async Task<ErrorResponseModel<string>> CreateSupplierAsync(SupplierRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var isExist = await _unitOfWork.Repository<Supplier>()
                .AnyAsync(x => x.AccountCode == request.AccountCode || x.Email == request.Email, cancellationToken);

            if (isExist)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var supplier = new Supplier
            {
                AccountCode = request.AccountCode,
                Name = request.Name,
                Address = request.Address,
                ResponsibleName1 = request.ResponsibleName1,
                ResponsibleName2 = request.ResponsibleName2,
                Phone1 = request.Phone1,
                Phone2 = request.Phone2,
                TaxNumber = request.TaxNumber,
                Job = request.Job,
                Fax1 = request.Fax1,
                Fax2 = request.Fax2,
                Email = request.Email,
                Website = request.Website,
                Notes = request.Notes
            };

            await _unitOfWork.Repository<Supplier>().AddAsync(supplier, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, supplier.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<SupplierResponse>>> GetAllSuppliersAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
                new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
                new SqlParameter("@PageSize", pagingFilter.PageSize)
            };

            var dt = await _sqlHelper.ExecuteDataTableAsync("dbo.SP_GetSuppliers", parameters);

            var suppliers = dt.AsEnumerable().Select(row => new SupplierResponse
            {
                Id = row.Field<int>("Id"),
                AccountCode = row.Field<string>("AccountCode") ?? string.Empty,
                Name = row.Field<string>("Name") ?? string.Empty,
                Address = row.Field<string>("Address") ?? string.Empty,
                ResponsibleName1 = row.Field<string>("ResponsibleName1") ?? string.Empty,
                ResponsibleName2 = row.Field<string>("ResponsibleName2"),
                Phone1 = row.Field<string>("Phone1") ?? string.Empty,
                Phone2 = row.Field<string>("Phone2"),
                TaxNumber = row.Field<string>("TaxNumber") ?? string.Empty,
                Job = row.Field<string>("Job"),
                Fax1 = row.Field<string>("Fax1"),
                Fax2 = row.Field<string>("Fax2"),
                Email = row.Field<string>("Email") ?? string.Empty,
                Website = row.Field<string>("Website"),
                Notes = row.Field<string>("Notes"),
                IsActive = row.Field<bool>("IsActive"),
                
            }).ToList();

            int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("TotalCount") : 0;

            return PagedResponseModel<List<SupplierResponse>>.Success(GenericErrors.GetSuccess, totalCount, suppliers);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<SupplierResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<SupplierResponse>> GetSupplierByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id, cancellationToken);

            if (supplier == null)
                return ErrorResponseModel<SupplierResponse>.Failure(GenericErrors.NotFound);

            var response = new SupplierResponse
            {
                Id = supplier.Id,
                AccountCode = supplier.AccountCode,
                Name = supplier.Name,
                Address = supplier.Address,
                ResponsibleName1 = supplier.ResponsibleName1,
                ResponsibleName2 = supplier.ResponsibleName2,
                Phone1 = supplier.Phone1,
                Phone2 = supplier.Phone2,
                TaxNumber = supplier.TaxNumber,
                Job = supplier.Job,
                Fax1 = supplier.Fax1,
                Fax2 = supplier.Fax2,
                Email = supplier.Email,
                Website = supplier.Website,
                Notes = supplier.Notes,
                IsActive = supplier.IsActive,
                Audit = new AuditResponse
                {
                    CreatedBy = supplier.CreatedById,
                    CreatedOn = supplier.CreatedOn,
                    UpdatedBy = supplier.UpdatedById,
                    UpdatedOn = supplier.UpdatedOn
                }
            };

            return ErrorResponseModel<SupplierResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<SupplierResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateSupplierAsync(int id, SupplierRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id, cancellationToken);

            if (supplier == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var isExist = await _unitOfWork.Repository<Supplier>()
                .AnyAsync(x => x.Id != id && (x.AccountCode == request.AccountCode || x.Email == request.Email), cancellationToken);

            if (isExist)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            supplier.AccountCode = request.AccountCode;
            supplier.Name = request.Name;
            supplier.Address = request.Address;
            supplier.ResponsibleName1 = request.ResponsibleName1;
            supplier.ResponsibleName2 = request.ResponsibleName2;
            supplier.Phone1 = request.Phone1;
            supplier.Phone2 = request.Phone2;
            supplier.TaxNumber = request.TaxNumber;
            supplier.Job = request.Job;
            supplier.Fax1 = request.Fax1;
            supplier.Fax2 = request.Fax2;
            supplier.Email = request.Email;
            supplier.Website = request.Website;
            supplier.Notes = request.Notes;

            _unitOfWork.Repository<Supplier>().Update(supplier);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, supplier.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> DeleteSupplierAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id, cancellationToken);

            if (supplier == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            supplier.IsActive = false;
            _unitOfWork.Repository<Supplier>().Update(supplier);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}
