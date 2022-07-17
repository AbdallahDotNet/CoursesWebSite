using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Faqs
    {
        public Guid Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
        public bool Is_active { get; set; }
    }
}
