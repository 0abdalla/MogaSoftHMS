using Azure;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Contracts.Appointments;
using Hospital_MS.Core.Migrations;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Page> GetManageRolePages()
        {
            try
            {
                var Pages = new List<Page>();
                var FlatPage = _unitOfWork.Repository<Page>().GetAll().ToList();
                var ParentPage = FlatPage.Where(p => p.ParentId == null && p.IsActive).OrderBy(i => i.DisplayOrder).ToList();
                foreach (var page in ParentPage)
                {
                    var Page = new Page();
                    Page.Id = page.Id;
                    Page.NameAR = page.NameAR;
                    Page.ParentId = page.ParentId;
                    Page.Icon = page.Icon;
                    Page.Route = page.Route;
                    Page.DisplayOrder = page.DisplayOrder;
                    Page.IsActive = page.IsActive;
                    Page.Children = FlatPage.Where(i => i.ParentId == page.Id && i.IsActive).OrderBy(i => i.DisplayOrder).ToList();

                    Pages.Add(Page);
                }
                return Pages;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Page>> GetAllowedPagesByRoleName(string RoleName)
        {
            try
            {
                if (string.IsNullOrEmpty(RoleName))
                    return null;

                var role = await _roleManager.FindByNameAsync(RoleName);
                var RoleId = Guid.Parse(role?.Id);

                var Pages = new List<Page>();
                var FlatPage = _unitOfWork.Repository<Page>().GetAll().ToList();
                var Permission = _unitOfWork.Repository<RolePermission>().GetAll(i => i.RoleId == RoleId).ToList();
                var results = (from page in FlatPage
                               join permission in Permission on page.Id equals permission.PageId
                               select new Page
                               {
                                   Id = page.Id,
                                   NameAR = page.NameAR,
                                   ParentId = page.ParentId,
                                   Icon = page.Icon,
                                   Route = page.Route,
                                   DisplayOrder = page.DisplayOrder,
                                   IsActive = page.IsActive
                               }).ToList();
                var ParentIds = results.Select(i => i.ParentId).Distinct().ToList();
                var PermissionPageIds = Permission.Select(i => i.PageId).Distinct().ToList();
                var ParentPages = FlatPage.Where(i => ParentIds.Contains(i.Id) && i.IsActive).OrderBy(i => i.DisplayOrder).ToList();
                foreach (var page in ParentPages)
                {
                    var Page = new Page();
                    Page.Id = page.Id;
                    Page.NameAR = page.NameAR;
                    Page.ParentId = page.ParentId;
                    Page.Icon = page.Icon;
                    Page.Route = page.Route;
                    Page.DisplayOrder = page.DisplayOrder;
                    Page.IsActive = page.IsActive;
                    Page.Children = FlatPage.Where(i => i.ParentId == page.Id && i.IsActive && PermissionPageIds.Contains(i.Id)).OrderBy(i => i.DisplayOrder).ToList();

                    Pages.Add(Page);
                }
                return Pages;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ErrorResponseModel<List<RolePermission>> GetPagesByRoleId(Guid RoleId)
        {
            var Pages = _unitOfWork.Repository<RolePermission>().GetAll(i => i.RoleId == RoleId).ToList();
            return ErrorResponseModel<List<RolePermission>>.Success(GenericErrors.UpdateSuccess, Pages);
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
