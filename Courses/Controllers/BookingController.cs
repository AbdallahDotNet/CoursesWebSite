using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.BookingVM;
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
    public class BookingController : Controller
    {
        private IBooking _repo;
        private INotification _repoNotification;
        private IGallery _repoGallery;
        private IApplicationIntro _repoAppIntro;
        private ISoical _repoSoical;
        public BookingController(IBooking repo,
            INotification repoNotification,
            IGallery repoGallery,
            IApplicationIntro repoAppIntro,
            ISoical repoSoical)
        {
            _repo = repo;
            _repoNotification = repoNotification;
            _repoAppIntro = repoAppIntro;
            _repoGallery = repoGallery;
            _repoSoical = repoSoical;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels {

                Get_notifications = await _repoNotification.GetAll(),
                Get_bookings = await _repo.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> BookingForm(Guid id)
        {
            var model = new ContainerViewModels
            {
                Course_id = id,
                Get_galleries = await _repoGallery.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Get_soical = (await _repoSoical.Get() == null) ? new() : await _repoSoical.Get()
            };
            return View(model);
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
                        TempData["Error"] = "Please Enter Code";
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

                if (await _repo.CheckExist(model.Save_booking.Student_name,
                    model.Save_booking.Course_id.Value))
                {
                    if (culture_name == "en")
                        TempData["Error"] = "You have booked this course before";
                    else
                        TempData["Error"] = "لقد حجزت هذا الكورس من قبل";
                    return LocalRedirect("/home/Error");
                }

                var result = await _repo.Save(model.Save_booking);
                if (!result)
                {
                    if (culture_name == "en")
                        TempData["Error"] = "Your data inValid";
                    else
                        TempData["Error"] = "البيانات التى ادخلتها غير صحيحه";
                    return LocalRedirect("/home/Error");
                }

                await _repoNotification.Save(new SaveNotificationViewModel {Content = "There is new booking"});

                if (culture_name == "en")
                    TempData["Success"] = "Booked successfully :)";
                else
                    TempData["Success"] = " ";//it's empty because sweetAlert not support arabic!
                return LocalRedirect("/home/Index");
            }
            catch (Exception e)
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
    }
}
