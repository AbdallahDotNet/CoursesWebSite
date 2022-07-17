using Interfaces.ViewModels.CourseSpecialtyVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICourseSpecialty
    {
        Task<List<GetCourseSpecialtyViewModel>> GetAll();
        Task<SaveCourseSpecialtyViewModel> GetUpdatedCourseSpecialty(Guid id);
        Task<bool> Save(SaveCourseSpecialtyViewModel model);
    }
}
