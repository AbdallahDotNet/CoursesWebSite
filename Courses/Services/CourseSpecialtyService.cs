using AutoMapper;
using Entitis.Models;
using Interfaces.Interfaces;
using Interfaces.ViewModels.CourseSpecialtyVM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class CourseSpecialtyService : ICourseSpecialty
    {
        private DataContext _context;
        private ICoreBase _repoCore;
        private IMapper _mapper;
        public CourseSpecialtyService(DataContext context,
            ICoreBase repoCore,
            IMapper mapper)
        {
            _context = context;
            _repoCore = repoCore;
            _mapper = mapper;
        }

        public async Task<List<GetCourseSpecialtyViewModel>> GetAll()
        {
            var courses_specialties = await _context.Course_specialties.ToListAsync();
            return _mapper.Map<List<GetCourseSpecialtyViewModel>>(courses_specialties);
        }

        public async Task<SaveCourseSpecialtyViewModel> GetUpdatedCourseSpecialty(Guid id)
        {
            var course_specialty = await _context.Course_specialties
                    .FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SaveCourseSpecialtyViewModel>(course_specialty);
        }

        public async Task<bool> Save(SaveCourseSpecialtyViewModel model)
        {
            if (model.Id == Guid.Empty)
            {
                var course_specialty = _mapper.Map<Course_specialty>(model);
                course_specialty.Is_active = true;
                _repoCore.Add(course_specialty);
            }
            else
            {
                var course_specialty = await _context.Course_specialties
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                _mapper.Map(model, course_specialty);
            }

            await _repoCore.SaveAll();
            return true;
        }
    }
}
