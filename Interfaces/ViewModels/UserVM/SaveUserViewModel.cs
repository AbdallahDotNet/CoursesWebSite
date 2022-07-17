using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.UserVM
{
    public class SaveUserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public string New_password { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
