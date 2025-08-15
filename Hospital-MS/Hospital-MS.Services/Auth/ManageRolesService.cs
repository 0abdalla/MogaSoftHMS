using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.PagesRole;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.Auth
{
    public class ManageRolesService : IManageRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageRolesService(IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<RoleDto>> GetAllRoles()
        {
            var roleTranslations = new Dictionary<string, string>
            {
                { "SystemAdmin", "مدير النظام" },
                { "TopManagement", "الادارة العليا" },
                { "FinanceManager", "المدير المالي" },
                { "TechnicalManager", "المدير الفني" },
                { "HRManager", "المدير الاداري" },
                { "Accountant", "المحاسب" },
                { "StoreKeeper", "مسئول المخازن" },
                { "ReservationEmployee", "موظف الحجز" },
                { "Cashier", "موظف الخزينة" },
                { "Auditor", "المراجع" }
            };
            var allRoles = await _roleManager.Roles.ToListAsync();

            var results = allRoles.Select(r => new RoleDto
            {
                RoleId = r.Id,
                RoleNameEn = r.Name,
                RoleNameAr = roleTranslations.ContainsKey(r.Name) ? roleTranslations[r.Name] : r.Name
            }).ToList();
            return results;
        }

        public ErrorResponseModel<List<string>> GetPagesByRoleId(Guid RoleId)
        {
            var Pages = (from rolePer in _unitOfWork.Repository<RolePermission>().GetAll(i => i.RoleId == RoleId).ToList()
                         join page in _unitOfWork.Repository<Page>().GetAll().ToList() on rolePer.PageId equals page.Id
                         select page).Select(i => i.PageName).ToList();
            return ErrorResponseModel<List<string>>.Success(GenericErrors.UpdateSuccess, Pages);
        }

        public ErrorResponseModel<List<PagesRolePermesstion>> GetAllPages()
        {
            var Pages = _unitOfWork.Repository<Page>().GetAll().Select(i => new PagesRolePermesstion { PageId = i.Id, PageName = i.PageName }).ToList();
            return ErrorResponseModel<List<PagesRolePermesstion>>.Success(GenericErrors.UpdateSuccess, Pages);
        }

        public async Task<ErrorResponseModel<string>> AssignRoleToPages(AssignRole Model, CancellationToken cancellationToken)
        {
            try
            {
                var RolePages = await _unitOfWork.Repository<RolePermission>().GetAll(i => i.RoleId == Model.RoleId).ToListAsync();
                if (RolePages.Count > 0)
                    _unitOfWork.Repository<RolePermission>().DeleteRange(RolePages);

                foreach (var pageId in Model.PageIds)
                {
                    var RolePage = new RolePermission();
                    RolePage.PageId = pageId;
                    RolePage.RoleId = Model.RoleId;
                    _unitOfWork.Repository<RolePermission>().AddAsync(RolePage, cancellationToken);
                }
                await _unitOfWork.CompleteAsync();
                return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess);
            }
            catch (Exception ex)
            {
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }
    }
}
