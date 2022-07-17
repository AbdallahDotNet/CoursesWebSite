using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.EventVM
{
    public class SaveEventViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int? Timing { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Lat { get; set; }

        [Required]
        public string Long { get; set; }

        [Required]
        public string Address { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
