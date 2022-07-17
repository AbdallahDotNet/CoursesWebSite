using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.ApplicationIntroVM
{
    public class SaveApplicationIntroViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string About { get; set; }

        public string Logo { get; set; }

        [Required]
        public string Coin { get; set; }
        public IFormFile File { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
