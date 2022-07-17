using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Events
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Timing { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Lat { get; set; }

        [Required]
        public string Long { get; set; }

        [Required]
        public string Address { get; set; }
        public bool Is_active { get; set; }
    }
}
