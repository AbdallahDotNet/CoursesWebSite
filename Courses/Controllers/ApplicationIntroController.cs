using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.ApplicationIntroVM;
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
    public class ApplicationIntroController : Controller
    {
        private IApplicationIntro _repo;
        private INotification _repoNotification;
        private IWebHostEnvironment _webHost;
        public ApplicationIntroController(IApplicationIntro repo,
            INotification repoNotification,
            IWebHostEnvironment webHost)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels
            {
                Get_application_intro = await _repo.Get(),
                Get_notifications = await _repoNotification.GetAll()
            };
            
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels()
            {
                Save_application_intro = new SaveApplicationIntroViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repo.Get() == null) ? new() : await _repo.Get()
            };
            return View("AppIntroForm", model);
        }

        public async Task<IActionResult> Update()
        {
            var model = new ContainerViewModels()
            {
                Save_application_intro = await _repo.GetUpdatedApplicationIntro(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repo.Get() == null) ? new() : await _repo.Get()
            };
            return View("AppIntroForm", model);
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
                    model = new ContainerViewModels()
                    {
                        Save_application_intro = 
                            (model.Save_application_intro.Id == Guid.Empty)? new SaveApplicationIntroViewModel() : await _repo.GetUpdatedApplicationIntro(),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repo.Get() == null) ? new() : await _repo.Get()
                    };
                    return View("AppIntroForm", model);
                }

                var root = Path.Combine(_webHost.WebRootPath, "Upload/Logo");
                var result = await _repo.Save(model.Save_application_intro, root);

                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels()
                    {
                        Save_application_intro =
                            (model.Save_application_intro.Id == Guid.Empty) ? new SaveApplicationIntroViewModel() : await _repo.GetUpdatedApplicationIntro(),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repo.Get() == null) ? new() : await _repo.Get()
                    };
                    model.Save_application_intro.Error = result;
                    return View("AppIntroForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch(Exception e)
            {
                model = new ContainerViewModels()
                {
                    Save_application_intro =
                            (model.Save_application_intro.Id == Guid.Empty) ? new SaveApplicationIntroViewModel() : await _repo.GetUpdatedApplicationIntro(),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_application_intro = (await _repo.Get() == null) ? new() : await _repo.Get()
                };

                if (culture_name == "en")
                    model.Save_application_intro.Error = "Error While Proccessing!";
                else
                    model.Save_application_intro.Error = "خطأ اثناء المعالجه!";
                return View("AppIntroForm", model);
            }
        }
    }
}
