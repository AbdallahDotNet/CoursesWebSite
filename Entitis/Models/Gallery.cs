using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Gallery
    {
        public Guid Id { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Description { get; set; }
        public bool Is_active { get; set; }
    }
}
