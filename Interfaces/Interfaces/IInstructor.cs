using Interfaces.ViewModels.InstructorVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IInstructor
    {
        Task<List<GetInstructorViewModel>> GetAll();
        Task<SaveInstructorViewModel> GetUpdatedInstructor(Guid id);
        Task<string> Save(SaveInstructorViewModel model, string root, string culture_name);
    }
}
