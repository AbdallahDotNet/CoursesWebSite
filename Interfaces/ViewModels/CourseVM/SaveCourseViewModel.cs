using Entitis.Models;
using Interfaces.ViewModels.CourseSpecialtyVM;
using Interfaces.ViewModels.InstructorVM;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.CourseVM
{
    public class SaveCourseViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Course_details { get; set; }
        public string Image { get; set; }
        public int Rate { get; set; }

        [Required]
        public float Price { get; set; }
        public float Price_after_discount => (Discount_value != 0) ? (Price * Discount_value) / 100 : 0;
        public int Discount_value { get; set; }

        [Required]
        public Guid? Specialty_id { get; set; }

        [Required]
        public Guid? Instructor_id { get; set; }
        public IFormFile File { get; set; }
        public Instructors Instructors { get; set; }
        public Course_specialty Course_specialty { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
