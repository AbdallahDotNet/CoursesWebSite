using AutoMapper;
using Interfaces.Helper;
using Interfaces.Interfaces;
using Interfaces.ViewModels.CourseVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class CourseService : ICourse
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public CourseService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<bool> CheckExist(Guid id)
        {
            var is_course_exist = await _context.Courses.AnyAsync();
            return is_course_exist;
        }

        public async Task<bool> Delete(Guid id, string root)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            course.Is_active = false;

            await _repoCore.SaveAll();
            return true;
        }

        public async Task<List<GetCousrseViewModel>> GetAll()
        {
            var courses = await _context.Courses
                .Include(x => x.Instructors)
                .Include(x => x.Course_specialty)
                .ToListAsync();
            return _mapper.Map<List<GetCousrseViewModel>>(courses);
        }

        public async Task<PagedList<GetCousrseViewModel>> GetAllPagination(int page_number)
        {
            var course = _context.Courses
                .Include(x => x.Course_specialty)
                .Where(x => x.Is_active == true)
                .Select(x => new GetCousrseViewModel { 
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Rate = x.Rate,
                    Course_details = x.Course_details,
                    Discount_value = x.Discount_value,
                    Image = x.Image,
                    Course_specialty = x.Course_specialty
                });

            return await PagedList<GetCousrseViewModel>.CreateAsync(course, page_number, 9);
        }

        public async Task<PagedList<GetCousrseViewModel>> GetAllPaginationBySpesialtyId(Guid id, int page_number)
        {
            var course = _context.Courses
                .Include(x => x.Course_specialty)
                .Where(x => x.Specialty_id == id)
                .Select(x => new GetCousrseViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Rate = x.Rate,
                    Course_details = x.Course_details,
                    Discount_value = x.Discount_value,
                    Image = x.Image,
                    Course_specialty = x.Course_specialty
                });

            return await PagedList<GetCousrseViewModel>.CreateAsync(course, page_number, 9);
        }

        public async Task<SaveCourseViewModel> GetUpdatedCourse(Guid id)
        {
            var course = await _context.Courses
                .Include(x => x.Instructors)
                .Include(x => x.Course_specialty)
                .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveCourseViewModel>(course);
        }

        public async Task<string> Save(SaveCourseViewModel model, string root, string culture_name)
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

                if (model.Title.Length < 22)
                    if (culture_name == "en")
                        return "Title must not be less than 22 characters";
                    else
                        return "العنوان يجب ان لا يقل عن 22 حرف";

                if (model.Course_details.Length < 140)
                    if (culture_name == "en")
                        return "Description must not be less than 140 characters";
                    else
                        return "الوصف يجب ان لا يقل عن 140 حرف";

                var course = _mapper.Map<Entitis.Models.Courses>(model);
                course.Image = file_name;
                course.Is_active = true;

                _repoCore.Add(course);
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

                if (model.Title.Length < 22)
                    if (culture_name == "en")
                        return "Title must not be less than 22 characters";
                    else
                        return "العنوان يجب ان لا يقل عن 22 حرف";

                if (model.Course_details.Length < 140)
                    if (culture_name == "en")
                        return "Description must not be less than 140 characters";
                    else
                        return "الوصف يجب ان لا يقل عن 140 حرف";

                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, course);

                course.Image = (model.File == null) ? model.Image : file_name;
            }

            await _repoCore.SaveAll();
            return string.Empty;
        }

        public async Task<PagedList<GetCousrseViewModel>> Search(string key, int page_number)
        {
            var course = _context.Courses
                .Where(x => x.Title.Contains(key))
                .Include(x => x.Course_specialty)
                .Select(x => new GetCousrseViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Rate = x.Rate,
                    Course_details = x.Course_details,
                    Discount_value = x.Discount_value,
                    Image = x.Image,
                    Course_specialty = x.Course_specialty
                });

            return await PagedList<GetCousrseViewModel>.CreateAsync(course, page_number, 9);
        }
    }
}
