using AutoMapper;
using Entitis.Models;
using Interfaces.Interfaces;
using Interfaces.ViewModels.InstructorVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class InstructorService : IInstructor
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public InstructorService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<List<GetInstructorViewModel>> GetAll()
        {
            var instructors = await _context.Instructors.ToListAsync();
            return _mapper.Map<List<GetInstructorViewModel>>(instructors);
        }

        public async Task<SaveInstructorViewModel> GetUpdatedInstructor(Guid id)
        {
            var instructor = await _context.Instructors
                    .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveInstructorViewModel>(instructor);
        }

        public async Task<string> Save(SaveInstructorViewModel model, string root, string culture_name)
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
                        return "خطأ اثناء الرفع!";

                var instructor = _mapper.Map<Instructors>(model);
                instructor.Image = file_name;
                instructor.Is_active = true;

                _repoCore.Add(instructor);
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

                var instructor = await _context.Instructors
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, instructor);

                instructor.Image = (model.File == null) ? model.Image : file_name;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }
    }
}
