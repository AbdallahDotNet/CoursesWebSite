using Interfaces.Helper;
using Interfaces.ViewModels.BlogVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IBlog
    {
        Task<PagedList<GetBlogViewModel>> GetAllPagination(int Page_number);
        Task<PagedList<GetBlogViewModel>> Search(string key, int page_number);
        Task<List<GetBlogViewModel>> GetAll();
        Task<SaveBlogViewModel> GetUpdatedBlog(Guid id);
        Task<string> Save(SaveBlogViewModel model, string publisher, string root, string culture_name);
        Task<bool> Delete(Guid id, string root);
        Task<bool> CheckExist(Guid id);
    }
}
