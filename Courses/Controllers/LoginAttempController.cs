using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class LoginAttempController : Controller
    {
        private ILoginAttemp _repo;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        public LoginAttempController(ILoginAttemp repo,
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
                Get_login_attemps = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> RemoveBlock(Guid id)
        {
            try
            {
                await _repo.RemoveBlock(id);

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
