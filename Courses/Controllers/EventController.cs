using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.EventVM;
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
    public class EventController : Controller
    {
        private IEvent _repo;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        private ISoical _repoSoical;
        private IGallery _repoGallery;
        private IWebHostEnvironment _webHost;
        public EventController(IEvent repo,
            INotification repoNotification,
            IApplicationIntro repoAppIntro,
            ISoical repoSoical,
            IGallery repoGallery,
            IWebHostEnvironment webHost)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _repoAppIntro = repoAppIntro;
            _repoGallery = repoGallery;
            _repoSoical = repoSoical;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels
            { 
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_events = await _repo.GetAll() 
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var model = new ContainerViewModels
            {
                Get_events = await _repo.GetAll(),
                Get_galleries = await _repoGallery.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            if (!await _repo.CheckExist(id))
                return LocalRedirect("/home/Error");

            var model = new ContainerViewModels
            {
                Get_galleries = await _repoGallery.GetAll(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Save_event = await _repo.GetUpdatedEvent(id)
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_event = new SaveEventViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_events = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("EventForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            if (!await _repo.CheckExist(id))
                return LocalRedirect("/home/Error");

            var model = new ContainerViewModels
            {
                Save_event = await _repo.GetUpdatedEvent(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_events = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("EventForm", model);
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
                        Save_event = (model.Save_event.Id == Guid.Empty)?
                            new SaveEventViewModel() : await _repo.GetUpdatedEvent(model.Save_event.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_events = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("EventForm", model);
                }

                var root = Path.Combine(_webHost.WebRootPath, "Upload/Events");
                var result = await _repo.Save(model.Save_event, root, culture_name);

                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels
                    {
                        Save_event = (model.Save_event.Id == Guid.Empty) ?
                            new SaveEventViewModel() : await _repo.GetUpdatedEvent(model.Save_event.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_events = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    model.Save_event.Error = result;
                    return View("EventForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_event = (model.Save_event.Id == Guid.Empty) ?
                            new SaveEventViewModel() : await _repo.GetUpdatedEvent(model.Save_event.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_events = await _repo.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_event.Error = "Error While Proccessing";
                else
                    model.Save_event.Error = "خطأ اثناء المعالجه";
                return View("EventForm", model);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var root = Path.Combine(_webHost.WebRootPath, "Upload/Events");
                await _repo.Delete(id, root);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                return LocalRedirect("/home/Error");
            }
        }
    }
}
