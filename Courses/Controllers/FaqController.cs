using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.FaqVM;
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
    public class FaqController : Controller
    {
        private IFaq _repo;
        private INotification _repoNotification;
        private ICourseSpecialty _repoCourseSpecialty;
        private IGallery _repoGallery;
        private ISoical _repoSoical;
        private IApplicationIntro _repoAppIntro;
        public FaqController(IFaq repo,
            INotification repoNotification,
            ICourseSpecialty repoCourseSpecialty,
            IGallery repoGallery,
            ISoical repoSoical,
            IApplicationIntro repoAppIntro)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _repoCourseSpecialty = repoCourseSpecialty;
            _repoGallery = repoGallery;
            _repoSoical = repoSoical;
            _repoAppIntro = repoAppIntro;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels 
            {
                Get_notifications = await _repoNotification.GetAll(),
                Get_faqs = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var model = new ContainerViewModels
            {
                Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                Get_faqs = await _repo.GetAll(),
                Get_galleries = await _repoGallery.GetAll(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Save_faq = new SaveFaqViewModel(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_faqs = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("FaqForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ContainerViewModels
            {
                Save_faq = await _repo.GetUpdatedFaq(id),
                Get_notifications = await _repoNotification.GetAll(),
                Get_faqs = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("FaqForm", model);
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
                        Save_faq = (model.Save_faq.Id == Guid.Empty)?
                            new SaveFaqViewModel() : await _repo.GetUpdatedFaq(model.Save_faq.Id),
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_faqs = await _repo.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                    };
                    return View("FaqForm", model);
                }

                await _repo.Save(model.Save_faq);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                model = new ContainerViewModels
                {
                    Save_faq = (model.Save_faq.Id == Guid.Empty) ?
                            new SaveFaqViewModel() : await _repo.GetUpdatedFaq(model.Save_faq.Id),
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_faqs = await _repo.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
                };

                if (culture_name == "en")
                    model.Save_faq.Error = "Error While Proccessing!";
                else
                    model.Save_faq.Error = "خطأ اثناء المعالجه!";
                return View("FaqForm", model);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repo.Delete(id);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error"); ;
            }
            catch (Exception e)
            {
                return LocalRedirect("/home/Error");
            }
        }
    }
}
