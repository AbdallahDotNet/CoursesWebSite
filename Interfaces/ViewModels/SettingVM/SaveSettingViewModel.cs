using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.NotifySettingVM
{
    public class SaveSettingViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string New_password { get; set; }
        public int Time_difference { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
