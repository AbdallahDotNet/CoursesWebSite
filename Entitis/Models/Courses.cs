using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Courses
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Course_details { get; set; }

        [Required]
        public string Image { get; set; }
        public int Rate { get; set; }

        [Required]
        public float Price { get; set; }
        public int Discount_value { get; set; }
        public bool Is_active { get; set; }
        public Guid Specialty_id { get; set; }
        [ForeignKey(nameof(Specialty_id))]
        public Course_specialty Course_specialty { get; set; }

        public Guid Instructor_id { get; set; }
        [ForeignKey(nameof(Instructor_id))]
        public Instructors Instructors { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
