using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Common;
using Hospital_MS.Core.Contracts.Customers;
using Hospital_MS.Core.Contracts.PurchaseOrder;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS;
public class CustomerService(IUnitOfWork unitOfWork, ISQLHelper sqlHelper) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ISQLHelper _sqlHelper = sqlHelper;

    public async Task<ErrorResponseModel<string>> CreateAsync(CustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingCustomer = await _unitOfWork.Repository<Customer>()
                .AnyAsync(x => x.Code == request.Code || x.Phone == request.Phone, cancellationToken);

            if (existingCustomer)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var customer = new Customer
            {
                Code = request.Code,
                Name = request.Name,
                NameEn = request.NameEn,
                ResponsibleName = request.ResponsibleName,
                ResponsibleName2 = request.ResponsibleName2,
                CustomerType = request.CustomerType,
                Job = request.Job,
                Region = request.Region,
                Phone = request.Phone,
                Phone2 = request.Phone2,
                Telephone = request.Telephone,
                Telephone2 = request.Telephone2,
                Email = request.Email,
                Notes = request.Notes,
                PaymentMethod = request.PaymentMethod,
                PaymentResponsible = request.PaymentResponsible,
                CreditLimit = request.CreditLimit,
                IsActive = true
            };

            await _unitOfWork.Repository<Customer>().AddAsync(customer, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, customer.Id.ToString());
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<PagedResponseModel<List<CustomerResponse>>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value),
                new SqlParameter("@CurrentPage", pagingFilter.CurrentPage),
                new SqlParameter("@PageSize", pagingFilter.PageSize)
            };

            var dt = await _sqlHelper.ExecuteDataTableAsync("Finance.SP_GetAllCustomers", parameters);

            var customers = dt.AsEnumerable().Select(row => new CustomerResponse
            {
                Id = row.Field<int>("Id"),
                Code = row.Field<string>("Code"),
                Name = row.Field<string>("Name"),
                NameEn = row.Field<string>("NameEn"),
                ResponsibleName = row.Field<string>("ResponsibleName"),
                ResponsibleName2 = row.Field<string>("ResponsibleName2"),
                CustomerType = row.Field<string>("CustomerType"),
                Job = row.Field<string>("Job"),
                Region = row.Field<string>("Region"),
                Phone = row.Field<string>("Phone"),
                Phone2 = row.Field<string>("Phone2"),
                Telephone = row.Field<string>("Telephone"),
                Telephone2 = row.Field<string>("Telephone2"),
                Email = row.Field<string>("Email"),
                Notes = row.Field<string>("Notes"),
                PaymentMethod = row.Field<string>("PaymentMethod"),
                PaymentResponsible = row.Field<string>("PaymentResponsible"),
                CreditLimit = row.Field<decimal>("CreditLimit"),
                IsActive = row.Field<bool>("IsActive"),
                Audit = new AuditResponse
                {
                    CreatedBy = row.Field<string>("CreatedBy"),
                    CreatedOn = row.Field<DateTime>("CreatedOn"),
                }
            }).ToList();

            int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("TotalCount") : 0;

            return PagedResponseModel<List<CustomerResponse>>.Success(GenericErrors.GetSuccess, totalCount, customers);
        }
        catch (Exception)
        {
            return PagedResponseModel<List<CustomerResponse>>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<CustomerResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _unitOfWork.Repository<Customer>()
                .GetAll()
                .Include(x => x.CreatedBy)
                .Include(x => x.UpdatedBy)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (customer == null)
                return ErrorResponseModel<CustomerResponse>.Failure(GenericErrors.NotFound);

            var response = new CustomerResponse
            {
                Id = customer.Id,
                Code = customer.Code,
                Name = customer.Name,
                NameEn = customer.NameEn,
                ResponsibleName = customer.ResponsibleName,
                ResponsibleName2 = customer.ResponsibleName2,
                CustomerType = customer.CustomerType,
                Job = customer.Job,
                Region = customer.Region,
                Phone = customer.Phone,
                Phone2 = customer.Phone2,
                Telephone = customer.Telephone,
                Telephone2 = customer.Telephone2,
                Email = customer.Email,
                Notes = customer.Notes,
                PaymentMethod = customer.PaymentMethod,
                PaymentResponsible = customer.PaymentResponsible,
                CreditLimit = customer.CreditLimit,
                IsActive = customer.IsActive,
                Audit = new AuditResponse
                {
                    CreatedBy = customer.CreatedBy.UserName,
                    CreatedOn = customer.CreatedOn,
                    UpdatedBy = customer.UpdatedBy?.UserName,
                    UpdatedOn = customer.UpdatedOn
                }
            };

            return ErrorResponseModel<CustomerResponse>.Success(GenericErrors.GetSuccess, response);
        }
        catch (Exception)
        {
            return ErrorResponseModel<CustomerResponse>.Failure(GenericErrors.TransFailed);
        }
    }

    public async Task<ErrorResponseModel<string>> UpdateAsync(int id, CustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id, cancellationToken);

            if (customer == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            var existingCustomer = await _unitOfWork.Repository<Customer>()
                .AnyAsync(x => x.Id != id && (x.Code == request.Code || x.Phone == request.Phone), cancellationToken);

            if (existingCustomer)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            customer.Code = request.Code;
            customer.Name = request.Name;
            customer.NameEn = request.NameEn;
            customer.ResponsibleName = request.ResponsibleName;
            customer.ResponsibleName2 = request.ResponsibleName2;
            customer.CustomerType = request.CustomerType;
            customer.Job = request.Job;
            customer.Region = request.Region;
            customer.Phone = request.Phone;
            customer.Phone2 = request.Phone2;
            customer.Telephone = request.Telephone;
            customer.Telephone2 = request.Telephone2;
            customer.Email = request.Email;
            customer.Notes = request.Notes;
            customer.PaymentMethod = request.PaymentMethod;
            customer.PaymentResponsible = request.PaymentResponsible;
            customer.CreditLimit = request.CreditLimit;

            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
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
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id, cancellationToken);

            if (customer == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            customer.IsActive = false;
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess);
        }
        catch (Exception)
        {
            return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
        }
    }
}