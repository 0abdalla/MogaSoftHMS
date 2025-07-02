using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Interfaces.Common;
public interface INotificationService
{
    Task SendNewPurchaseRequestNotification(int purchaseId);
}
