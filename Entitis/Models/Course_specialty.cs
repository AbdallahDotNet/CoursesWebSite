using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Course_specialty
    {
        public Guid Id { get; set; }

        [Required]
        public string Specialty { get; set; }

        public bool Is_active { get; set; }
        public ICollection<Courses> Courses { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}
