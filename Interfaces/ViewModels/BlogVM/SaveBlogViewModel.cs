using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.BlogVM
{
    public class SaveBlogViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }
        public DateTime Posted_date { get; set; }

        [Required]
        public string Content { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
