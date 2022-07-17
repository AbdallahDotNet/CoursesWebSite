using Interfaces.ViewModels.NotificationVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface INotification
    {
        Task<List<GetNotificationViewModel>> GetAll();
        Task<bool> Save(SaveNotificationViewModel model);
        Task<bool> UpdateSeen();
        int GetNotificationUnSeenCount();
    }
}
