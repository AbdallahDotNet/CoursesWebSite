using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Instructors
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Short_description { get; set; }

        [Required]
        public string Specialty { get; set; }

        [Required]
        public string Image { get; set; }
        public bool Is_active { get; set; }
        public int Rate { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public ICollection<Courses> Courses { get; set; }
    }
}
