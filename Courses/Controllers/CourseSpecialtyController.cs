using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.CourseSpecialtyVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CourseSpecialtyController : Controller
    {
        private ICourseSpecialty _repo;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        public CourseSpecialtyController(ICourseSpecialty repo,
            INotification repoNotification,
            IApplicationIntro repoAppIntro)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _repoAppIntro = repoAppIntro;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels 
            {
                Get_notifications = await _repoNotification.GetAll(),
                Get_course_specialtys = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_course_specialty = new SaveCourseSpecialtyViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_course_specialtys = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("CourseSpecialtyForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ContainerViewModels
            {
                Save_course_specialty = await _repo.GetUpdatedCourseSpecialty(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_course_specialtys = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("CourseSpecialtyForm", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(ContainerViewModels model)
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            try
            {
                if (!ModelState.IsValid)
                {
                    model = new ContainerViewModels
                    {
                        Save_course_specialty = (model.Save_course_specialty.Id == Guid.Empty)?
                            new SaveCourseSpecialtyViewModel() : await _repo.GetUpdatedCourseSpecialty(model.Save_course_specialty.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_course_specialtys = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("CourseSpecialtyForm", model);
                }

                await _repo.Save(model.Save_course_specialty);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch(Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_course_specialty = (model.Save_course_specialty.Id == Guid.Empty) ?
                            new SaveCourseSpecialtyViewModel() : await _repo.GetUpdatedCourseSpecialty(model.Save_course_specialty.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_course_specialtys = await _repo.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_course_specialty.Error = "Error While Proccessing!";
                else
                    model.Save_course_specialty.Error = "خطأ اثناء المعالجه!";
                return View("CourseSpecialtyForm", model);
            }
        }
    }
}
