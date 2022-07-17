using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.InstructorVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InstructorController : Controller
    {
        private IInstructor _repo;
        private INotification _repoNotification;
        private IWebHostEnvironment _webHost;
        private IApplicationIntro _repoAppIntro;
        public InstructorController(IInstructor repo,
            INotification repoNotification,
            IApplicationIntro repoAppIntro,
            IWebHostEnvironment webHost)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _repoAppIntro = repoAppIntro;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels 
            { 
                Get_notifications = await _repoNotification.GetAll(),
                Get_instructors = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_instructor = new SaveInstructorViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_instructors = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("InstructorForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ContainerViewModels
            {
                Save_instructor = await _repo.GetUpdatedInstructor(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_instructors = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("InstructorForm", model);
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
                        Save_instructor = (model.Save_instructor.Id == Guid.Empty)?
                            new SaveInstructorViewModel() : await _repo.GetUpdatedInstructor(model.Save_instructor.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_instructors = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("InstructorForm", model);
                }

                if (model.Save_instructor.Rate > 5)
                {
                    model = new ContainerViewModels
                    {
                        Save_instructor = (model.Save_instructor.Id == Guid.Empty) ?
                            new SaveInstructorViewModel() : await _repo.GetUpdatedInstructor(model.Save_instructor.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_instructors = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };

                    if (culture_name == "en")
                        model.Save_instructor.Error = "Max Rate Is Five";
                    else
                        model.Save_instructor.Error = "اقصى تقييم هو خمسه";
                    return View("InstructorForm", model);
                }

                var root = Path.Combine(_webHost.WebRootPath, "Upload/Instructors");
                var result = await _repo.Save(model.Save_instructor, root, culture_name);

                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels
                    {
                        Save_instructor = (model.Save_instructor.Id == Guid.Empty) ?
                            new SaveInstructorViewModel() : await _repo.GetUpdatedInstructor(model.Save_instructor.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_instructors = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    model.Save_instructor.Error = result;

                    return View("InstructorForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch(Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_instructor = (model.Save_instructor.Id == Guid.Empty) ?
                            new SaveInstructorViewModel() : await _repo.GetUpdatedInstructor(model.Save_instructor.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_instructors = await _repo.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_instructor.Error = "Error While Proccessing!";
                else
                    model.Save_instructor.Error = "خطأ اثناء المعالجه!";
                return View("InstructorForm", model);
            }
        }
    }
}
