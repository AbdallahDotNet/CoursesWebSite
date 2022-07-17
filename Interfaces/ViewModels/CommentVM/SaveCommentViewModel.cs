using Interfaces.ViewModels.CourseSpecialtyVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.CommentVM
{
    public class SaveCommentViewModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public string Student_name { get; set; }

        [Required]
        public string Content { get; set; }
        public bool Approval { get; set; }

        [Required]
        public Guid? Course_specialty_id { get; set; }
        public List<GetCourseSpecialtyViewModel> GetCoursesSpecialties { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
