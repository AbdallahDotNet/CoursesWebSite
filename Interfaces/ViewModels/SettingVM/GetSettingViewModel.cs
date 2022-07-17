using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.NotifySettingVM
{
    public class GetSettingViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Time_difference { get; set; }
    }
}
