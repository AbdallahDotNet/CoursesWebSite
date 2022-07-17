using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.SoicalVM;
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
    public class SoicalController : Controller
    {
        private ISoical _repo;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        public SoicalController(ISoical repo,
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
                Get_soical = await _repo.Get(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_soical = new SaveSoicalViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_soical = await _repo.Get(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("SoicalForm", model);
        }

        public async Task<IActionResult> Update()
        {
            var model = new ContainerViewModels
            {
                Save_soical = await _repo.GetUpdateSoical(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_soical = await _repo.Get(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("SoicalForm", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(ContainerViewModels model)
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            try
            {
                await _repo.Save(model.Save_soical);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_soical = (model.Save_soical.Id == Guid.Empty)?
                        new SaveSoicalViewModel() : await _repo.GetUpdateSoical(),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_soical = await _repo.Get(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_soical.Error = "Error While Proccessing!";
                else
                    model.Save_soical.Error = "خطأ اثناء المعالجه!";
                return View("SoicalForm", model);
            }
        }
    }
}
