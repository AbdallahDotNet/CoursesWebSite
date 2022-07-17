using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.InstructorVM
{
    public class SaveInstructorViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Short_description { get; set; }

        [Required]
        public string Specialty { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }
        public int Rate { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
