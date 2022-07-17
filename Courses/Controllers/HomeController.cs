using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    public class HomeController : Controller
    {
        private ICourse _repoCourse;
        private IInstructor _repoInstructor;
        private IBlog _repoBlog;
        private IEvent _repoEvent;
        private IComment _repoComment;
        private IApplicationIntro _repoAppIntro;
        private ISoical _repoSoical;
        private IGallery _repoGallery;
        public HomeController(ICourse repoCourse,
            IInstructor repoInstructor,
            IBlog repoBlog,
            IEvent repoEvent,
            IComment repoComment,
            IApplicationIntro repoAppIntro,
            ISoical repoSoical,
            IGallery repoGallery)
        {
            _repoCourse = repoCourse;
            _repoInstructor = repoInstructor;
            _repoBlog = repoBlog;
            _repoEvent = repoEvent;
            _repoComment = repoComment;
            _repoAppIntro = repoAppIntro;
            _repoSoical = repoSoical;
            _repoGallery = repoGallery;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"];
            else
                ViewBag.Success = null;

            var model = new ContainerViewModels
            {
                Get_blogs = await _repoBlog.GetAll(),
                Get_events = await _repoEvent.GetAll(),
                Get_instructors = await _repoInstructor.GetAll(),
                Get_cousrses = await _repoCourse.GetAll(),
                Get_comments = await _repoComment.GetAll(),
                Get_galleries = await _repoGallery.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> About()
        {
            var model = new ContainerViewModels
            {
                Get_cousrses = await _repoCourse.GetAll(),
                Get_comments = await _repoComment.GetAll(),
                Get_galleries = await _repoGallery.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null) ? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Error()
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"];
            else
                if (culture_name == "en")
                    ViewBag.Error = "Something went wrong";
                else
                    ViewBag.Error = "حدث خطأ ما";

            var model = new ContainerViewModels
            {
                Get_application_intro = await _repoAppIntro.Get()
            };
            return View(model);
        }
    }
}
