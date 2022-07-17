using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.NotificationVM
{
    public class SaveNotificationViewModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
