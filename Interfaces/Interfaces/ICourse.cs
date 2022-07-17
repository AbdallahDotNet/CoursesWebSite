using Interfaces.Helper;
using Interfaces.ViewModels.CourseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface ICourse
    {
        Task<PagedList<GetCousrseViewModel>> GetAllPaginationBySpesialtyId(Guid id, int page_number);
        Task<PagedList<GetCousrseViewModel>> GetAllPagination(int page_number);
        Task<List<GetCousrseViewModel>> GetAll();
        Task<SaveCourseViewModel> GetUpdatedCourse(Guid id);
        Task<string> Save(SaveCourseViewModel model, string root, string culture_name);
        Task<PagedList<GetCousrseViewModel>> Search(string key, int page_number);
        Task<bool> Delete(Guid id, string root);
        Task<bool> CheckExist(Guid id);
    }
}
