using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.MedicalServices;
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

namespace Hospital_MS.Services.HMS
{
    public class MedicalServiceService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper) : IMedicalServiceService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISQLHelper _sQLHelper = sQLHelper;

        public async Task<ErrorResponseModel<string>> CreateMedicalService(MedicalServiceRequest request, CancellationToken cancellationToken = default)
        {
            var isExist = await _unitOfWork.Repository<MedicalService>().AnyAsync(x => x.Name == request.Name, cancellationToken);

            if (isExist)
                return ErrorResponseModel<string>.Failure(GenericErrors.AlreadyExists);

            var medicalService = new MedicalService
            {
                Name = request.Name,
                Price = request.Price,
                Type = request.Type,
            };

            await _unitOfWork.Repository<MedicalService>().AddAsync(medicalService, cancellationToken);

            var result = await _unitOfWork.CompleteAsync(cancellationToken);

            if (result > 0)
                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, medicalService.Id.ToString());

            return ErrorResponseModel<string>.Failure(GenericErrors.GetSuccess);
        }

        public async Task<PagedResponseModel<DataTable>> GetAllAsync(PagingFilterModel pagingFilter, CancellationToken cancellationToken = default)
        {
            try
            {
                // Define SQL parameters
                var parameters = new SqlParameter[6];
                var status = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Status")?.ItemValue;
                var fromDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.FromDate;
                var toDate = pagingFilter.FilterList.FirstOrDefault(i => i.CategoryName == "Date")?.ToDate;

                parameters[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                parameters[1] = new SqlParameter("@Status", status ?? (object)DBNull.Value);
                parameters[2] = new SqlParameter("@FromDate", fromDate ?? (object)DBNull.Value);
                parameters[3] = new SqlParameter("@ToDate", toDate ?? (object)DBNull.Value);
                parameters[4] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                parameters[5] = new SqlParameter("@PageSize", pagingFilter.PageSize);

                // Execute stored procedure and get DataTable
                var dataTable = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetMedicalServices", parameters);

                int totalCount = 0;
                if (dataTable.Rows.Count > 0)
                {
                    int.TryParse(dataTable.Rows[0]["TotalCount"]?.ToString(), out totalCount);
                }
             
                // Return paginated response
                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dataTable);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }

        }

    }
}
