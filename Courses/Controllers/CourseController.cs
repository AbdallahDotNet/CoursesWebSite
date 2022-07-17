using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.CourseVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CourseController : Controller
    {
        private ICourse _repo;
        private ICoreBase _repoCore;
        private IInstructor _repoInstructor;
        private INotification _repoNotification;
        private ICourseSpecialty _repoCourseSpecialty;
        private IApplicationIntro _repoAppIntro;
        private ISoical _repoSoical;
        private IGallery _repoGallery;
        private IWebHostEnvironment _webHost;
        public CourseController(ICourse repo,
            ICoreBase repoCore,
            IInstructor repoInstructor,
            INotification repoNotification,
            ICourseSpecialty repoCourseSpecialty,
            IApplicationIntro repoAppIntro,
            ISoical repoSoical,
            IGallery repoGallery,
            IWebHostEnvironment webHost)
        {
            _repo = repo;
            _repoCore = repoCore;
            _repoInstructor = repoInstructor;
            _repoNotification = repoNotification;
            _repoCourseSpecialty = repoCourseSpecialty;
            _repoAppIntro = repoAppIntro;
            _repoSoical = repoSoical;
            _repoGallery = repoGallery;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels 
            { 
                Get_notifications = await _repoNotification.GetAll(),
                Get_cousrses = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAll(Guid specialtyId ,int pageNumber = 0)
        {
            var model = new ContainerViewModels
            {
                Get_cousrses = await _repo.GetAll(),
                Get_cousrses_pagination = (specialtyId == Guid.Empty)? await _repo.GetAllPagination(pageNumber) : await _repo.GetAllPaginationBySpesialtyId(specialtyId, pageNumber),
                Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                Get_galleries = await _repoGallery.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get(),
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            if (!await _repo.CheckExist(id))
                return RedirectToAction("Error", "Home");

            var model = new ContainerViewModels
            {
                Get_galleries = await _repoGallery.GetAll(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Save_course = await _repo.GetUpdatedCourse(id)
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels { 

                Save_course = new SaveCourseViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_instructors = await _repoInstructor.GetAll(),
                Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("CourseForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ContainerViewModels
            {
                Save_course = await _repo.GetUpdatedCourse(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_instructors = await _repoInstructor.GetAll(),
                Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("CourseForm", model);
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
                        Save_course = (model.Save_course.Id == Guid.Empty)?
                            new SaveCourseViewModel() : await _repo.GetUpdatedCourse(model.Save_course.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_instructors = await _repoInstructor.GetAll(),
                        Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("CourseForm", model);
                }

                if (model.Save_course.Rate > 5)
                {
                    model = new ContainerViewModels
                    {
                        Save_course = (model.Save_course.Id == Guid.Empty) ?
                            new SaveCourseViewModel() : await _repo.GetUpdatedCourse(model.Save_course.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_instructors = await _repoInstructor.GetAll(),
                        Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };

                    if (culture_name == "en")
                        model.Save_course.Error = "Max Rate Is Five";
                    else
                        model.Save_course.Error = "اقصى تقييم هو خمسه";
                    return View("CourseForm", model);
                }

                var root = Path.Combine(_webHost.WebRootPath, "Upload/Courses");
                var result = await _repo.Save(model.Save_course, root, culture_name);

                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels
                    {
                        Save_course = (model.Save_course.Id == Guid.Empty) ?
                            new SaveCourseViewModel() : await _repo.GetUpdatedCourse(model.Save_course.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_instructors = await _repoInstructor.GetAll(),
                        Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    model.Save_course.Error = result;
                    return View("CourseForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch(Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_course = (model.Save_course.Id == Guid.Empty) ?
                            new SaveCourseViewModel() : await _repo.GetUpdatedCourse(model.Save_course.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_instructors = await _repoInstructor.GetAll(),
                    Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_course.Error = "Error While Proccessing";
                else
                    model.Save_course.Error = "خطأ اثناء المعالجه";
                return View("CourseForm", model);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(SearchCourseViewModel search, int pageNumber = 0)
        {
            try
            {
                var reg = new Regex(@"^[^<>,?;:'()!~%$\-_@#/*""\s]+$");
                if (!reg.IsMatch(search.Key))
                {
                    return LocalRedirect("/home/Error"); ;
                }

                var model = new ContainerViewModels
                {
                    Get_cousrses = await _repo.GetAll(),
                    Get_cousrses_pagination = await _repo.Search(search.Key, pageNumber),
                    Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                    Get_galleries = await _repoGallery.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                    Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
                };
                return View("GetAll", model);
            }
            catch (Exception e)
            {
                return LocalRedirect("/home/Error"); ;
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var root = Path.Combine(_webHost.WebRootPath, "Upload/Courses");
                await _repo.Delete(id, root);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch(Exception e)
            {
                return LocalRedirect("/home/Error");
            }
        }
    }
}
