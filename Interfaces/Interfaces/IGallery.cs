using Interfaces.ViewModels.GalleryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IGallery
    {
        Task<List<GetGalleryViewModel>> GetAll();
        Task<SaveGalleryViewModel> GetUpdatedGallery(Guid id);
        Task<string> Save(SaveGalleryViewModel model, string root, string culture_name);
        Task<bool> Delete(Guid id, string root);
    }
}
