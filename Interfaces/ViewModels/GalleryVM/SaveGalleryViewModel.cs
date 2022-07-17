using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.GalleryVM
{
    public class SaveGalleryViewModel
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }

        [Required]
        public string Description { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
