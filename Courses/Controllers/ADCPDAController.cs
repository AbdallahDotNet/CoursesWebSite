using Interfaces.Helper;
using Interfaces.Interfaces;
using Interfaces.ViewModels;
using Interfaces.ViewModels.NotificationVM;
using Interfaces.ViewModels.UserVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Courses.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ADCPDAController : Controller
    {
        private IUser _repo;
        private ILoginAttemp _repoLoginAttemp;
        private IEvent _repoEvent;
        private ICourse _repoCourse;
        private IBlog _repoBlog;
        private INotification _repoNotification;
        private ICoreBase _repoCore;
        private IApplicationIntro _repoAppIntro;
        public ADCPDAController(IUser repo,
            ILoginAttemp repoLoginAttemp,
            IEvent repoEvent,
            ICourse repoCourse,
            IBlog repoBlog,
            INotification repoNotification,
            ICoreBase repoCore,
            IApplicationIntro repoAppIntro)
        {
            _repo = repo;
            _repoLoginAttemp = repoLoginAttemp;
            _repoEvent = repoEvent;
            _repoCourse = repoCourse;
            _repoBlog = repoBlog;
            _repoNotification = repoNotification;
            _repoCore = repoCore;
            _repoAppIntro = repoAppIntro;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ContainerViewModels {
                Get_users = await _repo.GetAll(),
                Get_cousrses = await _repoCourse.GetAll(),
                Get_blogs = await _repoBlog.GetAll(),
                Get_events = await _repoEvent.GetAll(),
                Get_notifications = await _repoNotification.GetAll(),
                Get_application_intro = (await _repoAppIntro.Get() == null) ? new() : await _repoAppIntro.Get()
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            if (await _repoLoginAttemp.CheckIpBlocked(HttpContext.Connection.RemoteIpAddress.ToString()))
            {
                if (culture_name == "en")
                    TempData["Error"] = "You are blocked";
                else
                    TempData["Error"] = "لقد تم حظرك";
                return LocalRedirect("/home/error");
            }

            if (!User.Identity.IsAuthenticated)
            {
                var model = new LoginViewModel();
                return View(model);
            }

            if (Url.IsLocalUrl("/"))
                return RedirectToAction(nameof(Index));

            return LocalRedirect("/home/error");
        }

        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string captchaCode)
        {
            var culture_request = HttpContext.Features.Get<IRequestCultureFeature>();
            var culture_name = culture_request.RequestCulture.UICulture.Name;

            try
            {
                if (string.IsNullOrEmpty(captchaCode))
                {
                    if (culture_name == "en")
                        model.Error = "Please Enter Code!";
                    else
                        model.Error = "برجاء ادخل الكود!";
                    return View(model);
                }

                if (!Captcha.ValidateCaptchaCode(captchaCode, HttpContext))
                {
                    if (culture_name == "en")
                        model.Error = "Code is Wrong!";
                    else
                        model.Error = "الكود خطأ!";
                    return View(model);
                }

                if (!ModelState.IsValid)
                    return View(model);

                var result = await _repo.Login(model);

                if (result == null)
                {
                    await _repoNotification.Save(new SaveNotificationViewModel {Content = "Login Attemp Failed By IP Address : " + HttpContext.Connection.RemoteIpAddress.ToString() });
                    await _repoLoginAttemp
                    .SaveLoginAttemps(HttpContext.Connection.RemoteIpAddress.ToString());

                    if (await _repoLoginAttemp.CheckIpBlocked(HttpContext.Connection.RemoteIpAddress.ToString()))
                    {
                        if (Url.IsLocalUrl("/"))
                            return RedirectToAction(nameof(Login));

                        return LocalRedirect("/home/error");
                    }

                    if (culture_name == "en")
                        model.Error = "Name Or Password Wrong!";
                    else
                        model.Error = "الاسم او كلمه المرور خطأ!";
                    return View(model);
                }

                HttpContext.Session
                    .SetString("JWToken",
                    new JwtSecurityTokenHandler().WriteToken(result.Token));

                if(Url.IsLocalUrl("/"))
                    return RedirectToAction(nameof(Index));

                return LocalRedirect("/home/error");

            }
            catch(Exception e)
            {
                if (culture_name == "en")
                    model.Error = "Error While Proccessing!";
                else
                    model.Error = "خطأ اثناء المعالجه!";
                return View(model);
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            if (Url.IsLocalUrl("/"))
                return RedirectToAction(nameof(Login));

            return LocalRedirect("/home/error");
        }

        [AllowAnonymous]
        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }

    public static class Captcha
    {
        const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateCaptchaCode()
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        public static bool ValidateCaptchaCode(string userInputCaptcha, HttpContext context)
        {
            var isValid = userInputCaptcha == context.Session.GetString("CaptchaCode");
            context.Session.Remove("CaptchaCode");
            return isValid;
        }

        public static CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random rand = new Random();

                graph.Clear(GetRandomLightColor());

                DrawCaptchaCode();
                DrawDisorderLine();
                AdjustRippleEffect();

                MemoryStream ms = new MemoryStream();

                baseMap.Save(ms, ImageFormat.Png);

                return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };

                int GetFontSize(int imageWidth, int captchCodeCount)
                {
                    var averageSize = imageWidth / captchCodeCount;

                    return Convert.ToInt32(averageSize);
                }

                Color GetRandomDeepColor()
                {
                    int redlow = 160, greenLow = 100, blueLow = 160;
                    return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
                }

                Color GetRandomLightColor()
                {
                    int low = 180, high = 255;

                    int nRend = rand.Next(high) % (high - low) + low;
                    int nGreen = rand.Next(high) % (high - low) + low;
                    int nBlue = rand.Next(high) % (high - low) + low;

                    return Color.FromArgb(nRend, nGreen, nBlue);
                }

                void DrawCaptchaCode()
                {
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    int fontSize = GetFontSize(width, captchaCode.Length);
                    Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    for (int i = 0; i < captchaCode.Length; i++)
                    {
                        fontBrush.Color = GetRandomDeepColor();

                        int shiftPx = fontSize / 6;

                        float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                        int maxY = height - fontSize;
                        if (maxY < 0) maxY = 0;
                        float y = rand.Next(0, maxY);

                        graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                    }
                }

                void DrawDisorderLine()
                {
                    Pen linePen = new Pen(new SolidBrush(Color.Black), 3);
                    for (int i = 0; i < rand.Next(3, 5); i++)
                    {
                        linePen.Color = GetRandomDeepColor();

                        Point startPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                        Point endPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                        graph.DrawLine(linePen, startPoint, endPoint);

                        //Point bezierPoint1 = new Point(rand.Next(0, width), rand.Next(0, height));
                        //Point bezierPoint2 = new Point(rand.Next(0, width), rand.Next(0, height));

                        //graph.DrawBezier(linePen, startPoint, bezierPoint1, bezierPoint2, endPoint);
                    }
                }

                void AdjustRippleEffect()
                {
                    short nWave = 6;
                    int nWidth = baseMap.Width;
                    int nHeight = baseMap.Height;

                    Point[,] pt = new Point[nWidth, nHeight];

                    for (int x = 0; x < nWidth; ++x)
                    {
                        for (int y = 0; y < nHeight; ++y)
                        {
                            var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                            var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                            var newX = x + xo;
                            var newY = y + yo;

                            if (newX > 0 && newX < nWidth)
                            {
                                pt[x, y].X = (int)newX;
                            }
                            else
                            {
                                pt[x, y].X = 0;
                            }


                            if (newY > 0 && newY < nHeight)
                            {
                                pt[x, y].Y = (int)newY;
                            }
                            else
                            {
                                pt[x, y].Y = 0;
                            }
                        }
                    }

                    Bitmap bSrc = (Bitmap)baseMap.Clone();

                    BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    int scanline = bitmapData.Stride;

                    IntPtr scan0 = bitmapData.Scan0;
                    IntPtr srcScan0 = bmSrc.Scan0;

                    unsafe
                    {
                        byte* p = (byte*)(void*)scan0;
                        byte* pSrc = (byte*)(void*)srcScan0;

                        int nOffset = bitmapData.Stride - baseMap.Width * 3;

                        for (int y = 0; y < nHeight; ++y)
                        {
                            for (int x = 0; x < nWidth; ++x)
                            {
                                var xOffset = pt[x, y].X;
                                var yOffset = pt[x, y].Y;

                                if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                                {
                                    if (pSrc != null)
                                    {
                                        p[0] = pSrc[yOffset * scanline + xOffset * 3];
                                        p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
                                        p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                                    }
                                }

                                p += 3;
                            }
                            p += nOffset;
                        }
                    }

                    baseMap.UnlockBits(bitmapData);
                    bSrc.UnlockBits(bmSrc);
                    bSrc.Dispose();
                }
            }
        }
    }
}
