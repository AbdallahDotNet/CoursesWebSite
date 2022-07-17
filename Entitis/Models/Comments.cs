using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Comments
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Student_name { get; set; }

        [Required]
        public string Content { get; set; }
        public bool Approval { get; set; }
        public bool Is_active { get; set; }
        public Guid Course_specialty_id { get; set; }
        [ForeignKey(nameof(Course_specialty_id))]
        public Course_specialty Course_specialty { get; set; }
    }
}
