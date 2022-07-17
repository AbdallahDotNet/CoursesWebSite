using Interfaces.Helper;
using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.BlogVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class BlogController : Controller
    {
        private IBlog _repo;
        private INotification _repoNotification;
        private IGallery _repoGallery;
        private IApplicationIntro _repoAppIntro;
        private ISoical _repoSoical;
        private IHashString _hash;
        private IWebHostEnvironment _webHost;
        public BlogController(IBlog repo,
            INotification repoNotification,
            IGallery repoGallery,
            IApplicationIntro repoAppIntro,
            ISoical repoSoical,
            IHashString hash,
            IWebHostEnvironment webHost)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _repoGallery = repoGallery;
            _repoAppIntro = repoAppIntro;
            _repoSoical = repoSoical;
            _hash = hash;
            _webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels {

                Get_blogs = await _repo.GetAll(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = ((await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get() == null) ? new() : (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int pageNumber = 0)
        {
            var model = new ContainerViewModels
            {
                Get_galleries = await _repoGallery.GetAll(),
                Get_blogs = await _repo.GetAll(),
                Get_blogs_pagination = (await _repo.GetAll() == null)? null : await _repo.GetAllPagination(pageNumber),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            ViewBag.CurrentUrl = HttpContext.Request.GetDisplayUrl();
            
            if (!await _repo.CheckExist(id))
                return LocalRedirect("/home/Error");

            var model = new ContainerViewModels
            {
                Get_galleries = await _repoGallery.GetAll(),
                Get_blogs = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get(),
                Save_blog = await _repo.GetUpdatedBlog(id)
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels()
            {
                Save_blog = new SaveBlogViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("BlogForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            if (!await _repo.CheckExist(id))
                return RedirectToAction("Error", "Home");

            var model = new ContainerViewModels()
            {
                Save_blog = await _repo.GetUpdatedBlog(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("BlogForm", model);
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
                        Save_blog = (model.Save_blog.Id == Guid.Empty)?
                            new SaveBlogViewModel() : await _repo.GetUpdatedBlog(model.Save_blog.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("BlogForm", model);
                }

                var root = Path.Combine(_webHost.WebRootPath, "Upload/Blogs");
                var result = await _repo.Save(model.Save_blog, User.FindFirst(ClaimTypes.Name).Value, root, culture_name);

                if (!string.IsNullOrEmpty(result))
                {
                    model = new ContainerViewModels()
                    {
                        Save_blog = (model.Save_blog.Id == Guid.Empty) ?
                            new SaveBlogViewModel() : await _repo.GetUpdatedBlog(model.Save_blog.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    model.Save_blog.Error = result;
                    return View("BlogForm", model);
                }

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch(Exception e)
            {
                model = new ContainerViewModels()
                {
                    Save_blog = (model.Save_blog.Id == Guid.Empty) ?
                            new SaveBlogViewModel() : await _repo.GetUpdatedBlog(model.Save_blog.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_blog.Error = "Error While Proccessing!";
                else
                    model.Save_blog.Error = "خطأ اثناء المعالجه!";
                return View("BlogForm", model);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Search(SearchBlogViewModel search, int pageNumber = 0)
        {
            try
            {
                var reg = new Regex(@"^[^<>.,?;:'()!~%$\-_@#/*""\s]+$");
                if (!reg.IsMatch(search.Key))
                {
                    return LocalRedirect("/home/Error");
                }

                var model = new ContainerViewModels
                {
                    Get_galleries = await _repoGallery.GetAll(),
                    Get_blogs = await _repo.GetAll(),
                    Get_blogs_pagination = await _repo.Search(search.Key, pageNumber),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                    Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
                };
                return View("GetAll", model);
            }
            catch (Exception e)
            {
                return LocalRedirect("/home/Error");
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var root = Path.Combine(_webHost.WebRootPath, "Upload/Blogs");
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
