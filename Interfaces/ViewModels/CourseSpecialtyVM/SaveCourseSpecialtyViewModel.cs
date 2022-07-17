using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.CourseSpecialtyVM
{
    public class SaveCourseSpecialtyViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Specialty { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
