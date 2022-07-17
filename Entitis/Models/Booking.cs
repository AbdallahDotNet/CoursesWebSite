using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        [Required]
        public string Student_name { get; set; }

        [Required]
        public string Phone { get; set; }

        public bool Is_active { get; set; }

        public Guid Course_id { get; set; }
        [ForeignKey(nameof(Course_id))]
        public Courses Courses { get; set; }
    }
}
