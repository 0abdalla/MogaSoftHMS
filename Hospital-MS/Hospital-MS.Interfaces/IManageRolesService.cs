using Hospital_MS.Core.Common;
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
        List<Page> GetManageRolePages();
        Task<List<RoleDto>> GetAllRoles();
        ErrorResponseModel<List<RolePermission>> GetPagesByRoleId(Guid RoleId);
        Task<List<Page>> GetAllowedPagesByRoleName(string RoleName);
        Task<ErrorResponseModel<string>> AssignRoleToPages(AssignRole Model, CancellationToken cancellationToken);
    }
}
