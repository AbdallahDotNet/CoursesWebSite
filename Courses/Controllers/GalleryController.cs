using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.GalleryVM;
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
    public class GalleryController : Controller
    {
        private IGallery _repo;
        private INotification _repoNotification;
        private IWebHostEnvironment _webHost;
        private IApplicationIntro _repoAppIntro;
        public GalleryController(IGallery repo,
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
                Get_galleries = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_gallery = new SaveGalleryViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_galleries = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("GalleryForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ContainerViewModels
            {
                Save_gallery = await _repo.GetUpdatedGallery(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_galleries = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("GalleryForm", model);
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
                        Save_gallery = (model.Save_gallery.Id == Guid.Empty)?
                            new SaveGalleryViewModel() : await _repo.GetUpdatedGallery(model.Save_gallery.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_galleries = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("GalleryForm", model);
                }

                var root = Path.Combine(_webHost.WebRootPath, "Upload/Gallery");
                var result = await _repo.Save(model.Save_gallery, root, culture_name);

                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels
                    {
                        Save_gallery = (model.Save_gallery.Id == Guid.Empty) ?
                            new SaveGalleryViewModel() : await _repo.GetUpdatedGallery(model.Save_gallery.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_galleries = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    model.Save_gallery.Error = result;
                    return View("GalleryForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_gallery = (model.Save_gallery.Id == Guid.Empty) ?
                            new SaveGalleryViewModel() : await _repo.GetUpdatedGallery(model.Save_gallery.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_galleries = await _repo.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_gallery.Error = "Error While Proccessing";
                else
                    model.Save_gallery.Error = "خطأ اثناء المعالجه";
                return View("GalleryForm", model);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var root = Path.Combine(_webHost.WebRootPath, "Upload/Gallery");
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
