using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.CourseSpecialtyVM
{
    public class GetCourseSpecialtyViewModel
    {
        public Guid Id { get; set; }
        public string Specialty { get; set; }
        public bool Is_active { get; set; }

    }
}
