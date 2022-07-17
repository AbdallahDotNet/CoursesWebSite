using AutoMapper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.GalleryVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class GalleryService : IGallery
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public GalleryService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<bool> Delete(Guid id, string root)
        {
            var gallery = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == id);
            gallery.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetGalleryViewModel>> GetAll()
        {
            var galleries = await _context.Galleries.ToListAsync();
            return _mapper.Map<List<GetGalleryViewModel>>(galleries);
        }

        public async Task<SaveGalleryViewModel> GetUpdatedGallery(Guid id)
        {
            var gallery = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveGalleryViewModel>(gallery);
        }

        public async Task<string> Save(SaveGalleryViewModel model, string root, string culture_name)
        {
            if (model.Id == Guid.Empty)
            {
                if (model.File == null)
                    if (culture_name == "en")
                        return "Please upload image!";
                    else
                        return "برجاء رفع صوره!";

                string file_name = null;
                var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                if (!upload)
                    if (culture_name == "en")
                        return "Error while uploading!";
                    else
                        return "خطأ اثناؤ الرفع!";

                var gallery = _mapper.Map<Entitis.Models.Gallery>(model);
                gallery.Image = file_name;
                gallery.Is_active = true;

                _repoCore.Add(gallery);
            }
            else
            {
                string file_name = null;

                if (model.File != null)
                {
                    var full_path = root + "/" + model.Image;
                    if (System.IO.File.Exists(full_path))
                    {
                        System.IO.File.Delete(full_path);
                    }

                    var upload = _repoCore.SaveSingleImage(root, model.File, out file_name);

                    if (!upload)
                        if (culture_name == "en")
                            return "Error while uploading!";
                        else
                            return "خطأ اثناء الرفع!";
                }

                var gallery = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, gallery);

                gallery.Image = (model.File == null) ? model.Image : file_name;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }
    }
}
