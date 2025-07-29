using Hangfire;
using Hospital_MS.Core.Common;
using Hospital_MS.Core.Common.Consts;
using Hospital_MS.Core.Contracts.PurchaseRequests;
using Hospital_MS.Core.Enums;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.HMS;
using Hospital_MS.Interfaces.Repository;
using Hospital_MS.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hospital_MS.Services.HMS
{
    public class PurchaseRequestService(IUnitOfWork unitOfWork,
        INotificationService notificationService,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor) : IPurchaseRequestService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly INotificationService _notificationService = notificationService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<ErrorResponseModel<string>> CreateAsync(PurchaseRequestRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await _userManager.FindByIdAsync(userId);

                var isAdmin = await _userManager.IsInRoleAsync(user, DefaultRoles.SystemAdmin.Name);

                var purchaseRequest = new PurchaseRequest
                {
                    RequestNumber = await GenerateRequestNumber(cancellationToken),
                    RequestDate = request.RequestDate,
                    DueDate = request.DueDate,
                    Purpose = request.Purpose,
                    StoreId = request.StoreId,
                    Notes = request.Notes,
                    Status = isAdmin ? PurchaseStatus.Approved : PurchaseStatus.Pending,
                    IsActive = true,
                    Items = request.Items.Select(i => new PurchaseRequestItem
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        Notes = i.Notes,
                        IsActive = true
                    }).ToList()
                };

                await _unitOfWork.Repository<PurchaseRequest>().AddAsync(purchaseRequest, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                if (!isAdmin)
                {
                    BackgroundJob.Enqueue(() => _notificationService.SendNewPurchaseRequestNotification(purchaseRequest.Id));
                }

                var notification = new Notification
                {
                    TargetId = purchaseRequest.Id,
                    Type = NotificationType.PurchaseRequest,
                    Status = purchaseRequest.Status.ToString(),
                    AdditionalInfo = "طلب شراء جديد تمت إضافته"
                };

                await _notificationService.CreateAndNotifyAsync(notification, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return ErrorResponseModel<string>.Success(GenericErrors.AddSuccess, purchaseRequest.RequestNumber);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ErrorResponseModel<string>.Failure(GenericErrors.TransFailed);
            }

        }

        public async Task<ErrorResponseModel<string>> UpdateAsync(int id, PurchaseRequestRequest request, CancellationToken cancellationToken = default)
        {
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .GetAll(x => x.Id == id && x.IsActive)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(cancellationToken);

            if (purchaseRequest == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            purchaseRequest.RequestDate = request.RequestDate;
            purchaseRequest.DueDate = request.DueDate;
            purchaseRequest.Purpose = request.Purpose;
            purchaseRequest.StoreId = request.StoreId;
            purchaseRequest.Notes = request.Notes;

            //foreach (var item in purchaseRequest.Items)
            //    item.IsActive = false;

            // Clear existing items and add new ones
            purchaseRequest.Items.Clear();

            purchaseRequest.Items = request.Items.Select(i => new PurchaseRequestItem
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                Notes = i.Notes,
                IsActive = true
            }).ToList();

            _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, purchaseRequest.Id.ToString());
        }

        public async Task<ErrorResponseModel<PurchaseRequestResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var pr = await _unitOfWork.Repository<PurchaseRequest>()
                .GetAll()
                .Include(x => x.Store)
                .Include(x => x.Items)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (pr == null)
                return ErrorResponseModel<PurchaseRequestResponse>.Failure(GenericErrors.NotFound);

            var response = new PurchaseRequestResponse
            {
                Id = pr.Id,
                RequestNumber = pr.RequestNumber,
                RequestDate = pr.RequestDate,
                DueDate = pr.DueDate,
                Purpose = pr.Purpose,
                StoreId = pr.StoreId,
                StoreName = pr.Store.Name,
                Status = pr.Status.ToString(),
                Notes = pr.Notes,
                Items = pr.Items.Where(i => i.IsActive).Select(i => new PurchaseRequestItemResponse
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    ItemName = i.Item.NameAr,
                    Quantity = i.Quantity,
                    Notes = i.Notes
                }).ToList()
            };

            return ErrorResponseModel<PurchaseRequestResponse>.Success(GenericErrors.GetSuccess, response);
        }

        public async Task<PagedResponseModel<List<PurchaseRequestResponse>>> GetAllAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Repository<PurchaseRequest>().GetAll()
                .Include(x => x.Store)
                .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.RequestNumber.Contains(filter.SearchText) || x.Purpose.Contains(filter.SearchText));

            var totalCount = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PurchaseRequestResponse
                {
                    Id = x.Id,
                    RequestNumber = x.RequestNumber,
                    RequestDate = x.RequestDate,
                    DueDate = x.DueDate,
                    Purpose = x.Purpose,
                    StoreName = x.Store.Name,
                    Status = x.Status.ToString(),
                    Notes = x.Notes,
                    Items = new()
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<PurchaseRequestResponse>>.Success(GenericErrors.GetSuccess, totalCount, list);
        }

        public async Task<ErrorResponseModel<string>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var pr = await _unitOfWork.Repository<PurchaseRequest>()
                .GetAll()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

            if (pr == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            pr.IsActive = false;
            foreach (var item in pr.Items)
                item.IsActive = false;

            _unitOfWork.Repository<PurchaseRequest>().Update(pr);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.DeleteSuccess, pr.Id.ToString());
        }

        public async Task<ErrorResponseModel<string>> ApprovePurchaseRequestAsync(int id, CancellationToken cancellationToken = default)
        {
            var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
                .GetAll(x => x.Id == id && x.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (purchaseRequest == null)
                return ErrorResponseModel<string>.Failure(GenericErrors.NotFound);

            purchaseRequest.Status = PurchaseStatus.Approved;

            _unitOfWork.Repository<PurchaseRequest>().Update(purchaseRequest);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return ErrorResponseModel<string>.Success(GenericErrors.UpdateSuccess, purchaseRequest.Id.ToString());
        }

        public async Task<PagedResponseModel<List<PurchaseRequestResponse>>> GetAllApprovedAsync(PagingFilterModel filter, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.Repository<PurchaseRequest>().GetAll()
                .Include(x => x.Store)
                .Where(x => x.IsActive && x.Status == PurchaseStatus.Approved);

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
                query = query.Where(x => x.RequestNumber.Contains(filter.SearchText) || x.Purpose.Contains(filter.SearchText));

            var totalCount = await query.CountAsync(cancellationToken);

            var list = await query
                .OrderByDescending(x => x.Id)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PurchaseRequestResponse
                {
                    Id = x.Id,
                    RequestNumber = x.RequestNumber,
                    RequestDate = x.RequestDate,
                    DueDate = x.DueDate,
                    Purpose = x.Purpose,
                    StoreName = x.Store.Name,
                    Status = x.Status.ToString(),
                    Notes = x.Notes,
                    Items = new()
                })
                .ToListAsync(cancellationToken);

            return PagedResponseModel<List<PurchaseRequestResponse>>.Success(GenericErrors.GetSuccess, totalCount, list);
        }



        private async Task<string> GenerateRequestNumber(CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year;
            var count = await _unitOfWork.Repository<PurchaseRequest>()
                .CountAsync(x => x.RequestDate.Year == year, cancellationToken);
            return $"PR-{year}-{(count + 1):D5}";
        }
    }
}