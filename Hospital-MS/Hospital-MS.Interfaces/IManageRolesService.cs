using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PagesRole;
using Hospital_MS.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces
{
    public interface IManageRolesService
    {
        Task<List<RoleDto>> GetAllRoles();
        ErrorResponseModel<List<string>> GetPagesByRoleId(Guid RoleId);
        ErrorResponseModel<List<PagesRolePermesstion>> GetAllPages();
        Task<ErrorResponseModel<string>> AssignRoleToPages(AssignRole Model, CancellationToken cancellationToken);
    }
}
