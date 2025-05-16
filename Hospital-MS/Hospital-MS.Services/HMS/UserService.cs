using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Doctors;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Services.HMS
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLHelper _sQLHelper;
        public UserService(IUnitOfWork unitOfWork, ISQLHelper sQLHelper)
        {
            _unitOfWork = unitOfWork;
            _sQLHelper = sQLHelper;
        }

        public async Task<PagedResponseModel<DataTable>> GetAllUsers(PagingFilterModel pagingFilter)
        {
            try
            {
                var Params = new SqlParameter[3];
                Params[0] = new SqlParameter("@SearchText", pagingFilter.SearchText ?? (object)DBNull.Value);
                Params[1] = new SqlParameter("@CurrentPage", pagingFilter.CurrentPage);
                Params[2] = new SqlParameter("@PageSize", pagingFilter.PageSize);

                var dt = await _sQLHelper.ExecuteDataTableAsync("dbo.SP_GetAllUsers", Params);

                int totalCount = dt.Rows.Count > 0 ? dt.Rows[0].Field<int?>("TotalCount") ?? 0 : 0;

                return PagedResponseModel<DataTable>.Success(GenericErrors.GetSuccess, totalCount, dt);
            }
            catch (Exception)
            {
                return PagedResponseModel<DataTable>.Failure(GenericErrors.TransFailed);
            }
        }

        public PagedResponseModel<List<UsersModel>> GetAllEmployees()
        {
            var results = _unitOfWork.Repository<Staff>().GetAll().Select(i => new UsersModel
            {
                UserId = i.Id,
                UserName = i.FullName
            }).ToList();

            return PagedResponseModel<List<UsersModel>>.Success(GenericErrors.GetSuccess, results.Count, results);
        }
    }
}
