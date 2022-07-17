using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.UserVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class UserController : Controller
    {
        private IUser _repo;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        public UserController(IUser repo,
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
                Get_users = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_user = new SaveUserViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("UserForm", model);
        }

        public async Task<IActionResult> Update(string id)
        {
            var model = new ContainerViewModels
            {
                Save_user = await _repo.GetUpdatedUser(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("UserForm", model);
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
                        Save_user = (string.IsNullOrEmpty(model.Save_user.Id))?
                            new SaveUserViewModel() : await _repo.GetUpdatedUser(model.Save_user.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("UserForm", model);
                }

                var result = await _repo.SaveUser(model.Save_user);
                if (!result)
                {
                    model = new ContainerViewModels
                    {
                        Save_user = (string.IsNullOrEmpty(model.Save_user.Id)) ?
                            new SaveUserViewModel() : await _repo.GetUpdatedUser(model.Save_user.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };

                    if (culture_name == "en")
                        model.Save_user.Error = "Password weak, Password must be contain capital letters, numbers and signs!";
                    else
                        model.Save_user.Error = "كلمه مرور ضعيفه, كلمه المرور يجب ان تحوى احرف كبيره و ارقام وعلامات!";
                    return View("UserForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_user = (string.IsNullOrEmpty(model.Save_user.Id)) ?
                            new SaveUserViewModel() : await _repo.GetUpdatedUser(model.Save_user.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_user.Error = "Error While Proccessing!";
                else
                    model.Save_user.Error = "خطأ اثناء المعالجه!";
                return View("UserForm", model);
            }
        }

        //public async Task<IActionResult> Delete(string id)
        //{
        //    try {

        //        await _repo.Delete(id);

        //        if (Url.IsLocalUrl("/"))
        //            return RedirectToAction(nameof(Index));

        //        return LocalRedirect("/home/error");
        //    }
        //    catch (Exception e)
        //    {
        //        return LocalRedirect("/home/Error");
        //    }
        //}
    }
}
