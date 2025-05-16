using Hospital_MS.Core.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.HMS
{
    public interface IUserService
    {
        Task<PagedResponseModel<DataTable>> GetAllUsers(PagingFilterModel pagingFilter);
        PagedResponseModel<List<UsersModel>> GetAllEmployees();
    }
}
