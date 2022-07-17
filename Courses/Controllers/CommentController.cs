using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.CommentVM;
using Interfaces.ViewModels.NotificationVM;
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
    public class CommentController : Controller
    {
        private IComment _repo;
        private ICourseSpecialty _repoCourseSpecialty;
        private IFaq _repoFaq;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        public CommentController(IComment repo,
            ICourseSpecialty repoCourseSpecialty,
            IFaq repoFaq,
            INotification repoNotification,
            IApplicationIntro repoAppIntro)
        {
            _repo = repo;
            _repoCourseSpecialty = repoCourseSpecialty;
            _repoFaq = repoFaq;
            _repoNotification = repoNotification;
            _repoAppIntro = repoAppIntro;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels { 

                Get_notifications = await _repoNotification.GetAll(),
                Get_comments =  await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            var model = new ContainerViewModels
            {
                Get_notifications = await _repoNotification.GetAll(),
                Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("CommentForm", model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = new ContainerViewModels { 

                Save_comment = await _repo.GetUpdatedComment(id),
                Get_course_specialtys = await _repoCourseSpecialty.GetAll(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View("CommentForm", model);
        }

        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(ContainerViewModels model, string captchaCode)
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            try
            {
                if (string.IsNullOrEmpty(captchaCode))
                {
                    if (culture_name == "en")
                        TempData["Error"] = "Please enter code";
                    else
                        TempData["Error"] = "برجاء ادخل الكود";
                    return LocalRedirect("/home/Error");
                }

                if (!Captcha.ValidateCaptchaCode(captchaCode, HttpContext))
                {
                    if (culture_name == "en")
                        TempData["Error"] = "Code is wrong";
                    else
                        TempData["Error"] = "الكود خطأ";
                    return LocalRedirect("/home/Error");
                }

                if (!ModelState.IsValid)
                {
                    if (culture_name == "en")
                        TempData["Error"] = "Check inputs fill Or your data inValid";
                    else
                        TempData["Error"] = "تأكد من الحقول ليست فارغه او ان البيانات التى ادخلتها غير صحيحه";
                    return LocalRedirect("/home/Error");
                }

                var result = await _repo.Save(model.Save_comment);
                if (!result)
                {
                    if (culture_name == "en")
                        TempData["Error"] = "Your data inValid";
                    else
                        TempData["Error"] = "البيانات التى ادخلتها غير صحيحه";
                    return LocalRedirect("/home/Error");
                }

                await _repoNotification.Save(new SaveNotificationViewModel {Content = "There are comments wating approve"});

                if (culture_name == "en")
                    TempData["Success"] = "Comment Published Please Wait Admin Approve :)";
                else
                    TempData["Success"] = " ";//it's empty because sweetAlert not support arabic!
                return LocalRedirect("/home/Index");

            }
            catch(Exception e)
            {
                if (culture_name == "en")
                    TempData["Error"] = "Error while proccessing";
                else
                    TempData["Error"] = "خطأ اثناء المعالجه";
                return LocalRedirect("/home/Error");
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repo.Delete(id);

                if (Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");
            }
            catch (Exception e)
            {
                return LocalRedirect("/home/Error");
            }
        }

        public async Task<IActionResult> Approve(Guid id)
        {
            try
            {
                await _repo.Approving(id);

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
