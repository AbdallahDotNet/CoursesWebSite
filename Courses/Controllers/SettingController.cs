using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.NotifySettingVM;
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
    public class SettingController : Controller
    {
        private ISetting _repo;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        public SettingController(ISetting repo,
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
                Get_setting = await _repo.Get(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null) ? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_setting = new SaveSettingViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = await _repoAppIntro.Get()
            };
            return View("SettingForm", model);
        }

        public async Task<IActionResult> Update()
        {
            var model = new ContainerViewModels
            {
                Save_setting = await _repo.GetUpdatedSetting(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = await _repoAppIntro.Get()
            };
            return View("SettingForm", model);
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
                        Save_setting = (model.Save_setting.Id == Guid.Empty)?
                            new SaveSettingViewModel() : await _repo.GetUpdatedSetting(),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = await _repoAppIntro.Get()
                    };
                    return View("SettingForm", model);
                }

                var result = await _repo.Save(model.Save_setting);
                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels
                    {
                        Save_setting = (model.Save_setting.Id == Guid.Empty) ?
                            new SaveSettingViewModel() : await _repo.GetUpdatedSetting(),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = await _repoAppIntro.Get()
                    };
                    model.Save_setting.Error = result;
                    return View("SettingForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_setting = (model.Save_setting.Id == Guid.Empty) ?
                            new SaveSettingViewModel() : await _repo.GetUpdatedSetting(),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_application_intro = await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_setting.Error = "Error While Proccessing";
                else
                    model.Save_setting.Error = "خطأ اثناء المعالجه";
                return View("SettingForm", model);
            }
        }
    }
}
