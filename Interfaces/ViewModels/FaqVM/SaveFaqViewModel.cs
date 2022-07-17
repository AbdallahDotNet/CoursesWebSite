using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.FaqVM
{
    public class SaveFaqViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
