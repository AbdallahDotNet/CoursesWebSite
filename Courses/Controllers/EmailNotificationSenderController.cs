using Interfaces.Helper;
using Interfaces.Interfaces;
using Interfaces.ViewModels;
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
    public class EmailNotificationSenderController : Controller
    {
        private IEmailSender _repo;
        private INewsLetter _repoNewsLetter;
        private ISetting _repoSetting;
        private INotification _repoNotification;
        private IApplicationIntro _repoAppIntro;
        private IHashString _hash;
        public EmailNotificationSenderController(IEmailSender repo,
            INewsLetter repoNewsLetter,
            ISetting repoSetting,
            INotification repoNotification,
            IApplicationIntro repoAppIntro,
            IHashString hash)
        {
            _repo = repo;
            _repoNewsLetter = repoNewsLetter;
            _repoSetting = repoSetting;
            _repoNotification = repoNotification;
            _repoAppIntro = repoAppIntro;
            _hash = hash;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels 
            {
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                Mail_request = new()
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Send(ContainerViewModels model)
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            try
            {
                if (string.IsNullOrEmpty(model.Mail_request.Subject))
                {
                    model = new ContainerViewModels
                    {
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                        Mail_request = new()
                    };

                    if (culture_name == "en")
                        model.Mail_request.Error = "Subject Can't be empty!";
                    else
                        model.Mail_request.Error = "الموضوع لا يمكن ان يكون فارغ!";
                    return View("Index", model);
                }

                if (string.IsNullOrEmpty(model.Mail_request.Body))
                {
                    model = new ContainerViewModels
                    {
                        Get_notifications = await _repoNotification.GetAll(),
                        Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                        Mail_request = new()
                    };

                    if (culture_name == "en")
                        model.Mail_request.Error = "Content Can't be empty!";
                    else
                        model.Mail_request.Error = "المحتوى لا يمكن ان يكون فارغ!";
                    return View("Index", model);
                }

                foreach (var item in await _repoNewsLetter.GetAll())
                {
                    var mail_request = new MailRequest { 
                        Subject = model.Mail_request.Subject,
                        Body = model.Mail_request.Body,
                        To_email = item.Email
                    };

                    var setting = await _repoSetting.Get();
                    var mail_setting = new MailSetting { 
                        Mail = setting.Email,
                        Password = _hash.Decrypt(setting.Password)
                    };

                    _repo.SendEmailAsync(mail_request, mail_setting);
                }

                if (culture_name == "en")
                    ViewBag.SendSuccessed = "Send Successfully";
                else
                    ViewBag.SendSuccessed = "نجح الارسال";

                model = new ContainerViewModels
                {
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                    Mail_request = new()
                };
                return View("Index", model);
            }
            catch(Exception e)
            {
                model = new ContainerViewModels
                {
                    Get_notifications = await _repoNotification.GetAll(),
                    Get_application_intro = (await _repoAppIntro.Get() == null)? new() : await _repoAppIntro.Get(),
                    Mail_request = new()
                };

                if (culture_name == "en")
                    model.Mail_request.Error = "Error While Sending, Check your email or password is right!";
                else
                    model.Mail_request.Error = "خطأ اثناء الارسال , تأكد من ان ايميلك و كلمه المرور صحيحه!";
                return View("Index", model);
            }
        }
    }
}
