using Hospital_MS.Core.Common.Consts;
using Hospital_MS.Core.Models;
using Hospital_MS.Interfaces.Common;
using Hospital_MS.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Hospital_MS.Services.Common;
public class NotificationService(IUnitOfWork unitOfWork,
     UserManager<ApplicationUser> userManager,
     IHttpContextAccessor httpContextAccessor,
     IEmailSender emailService) : INotificationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailService = emailService;


    public async Task SendNewPurchaseRequestNotification(int purchaseId)
    {

        var purchaseRequest = await _unitOfWork.Repository<PurchaseRequest>()
            .GetAll(x => x.Id == purchaseId)
            .Include(pr => pr.Store)
            .Include(pr => pr.Items).ThenInclude(pr => pr.Item)
            .FirstOrDefaultAsync();

        if (purchaseRequest == null)
            return;

        var adminUsers = await _userManager.GetUsersInRoleAsync(DefaultRoles.SystemAdmin.Name);
        if (adminUsers == null || !adminUsers.Any())
            return;

        var subject = $"New Purchase Request: {purchaseRequest.RequestNumber}";

        var templateModel = new Dictionary<string, string>
        {
            { "{RequestId}", purchaseRequest.Id.ToString() },
            { "{RequestNumber}", purchaseRequest.RequestNumber },
            { "{RequestDate}", purchaseRequest.RequestDate.ToString("yyyy-MM-dd") },
            { "{StoreName}", purchaseRequest.Store?.Name ?? string.Empty },
            { "{Purpose}", purchaseRequest.Purpose },
            { "{Status}", purchaseRequest.Status.ToString() },
            { "{Notes}", purchaseRequest.Notes ?? string.Empty },
            { "{Items}", string.Join("<br/>", purchaseRequest.Items.Select(i => $"- {i.Item.NameAr} ({i.Quantity})")) }

        };
        var body = EmailBodyBuilder.GenerateEmailBody("NewPurchaseRequest", templateModel);

        await _emailService.SendEmailAsync("magdeleslams@gmail.com", subject, body);

        // TODO: Uncomment the following lines to send emails to all admin users

        //foreach (var admin in adminUsers)
        //{
        //    if (!string.IsNullOrWhiteSpace(admin.Email))
        //    {
        //        await _emailService.SendEmailAsync(admin.Email, subject, body);
        //    }
        //}
    }
}


