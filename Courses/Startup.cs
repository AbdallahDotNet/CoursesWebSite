using Courses.Services;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Courses
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(20);
                options.Cookie.HttpOnly = true;
            });

            services.AddDbContext<DataContext>(op => 
                op.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), m => m.MigrationsAssembly("Courses"))
            );
            services.AddAutoMapper(typeof(Startup));
            services.AddIdentity<Entitis.Models.Users, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op => {

                op.SaveToken = true;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                };
            });

            services.AddScoped<ICoreBase, CoreBaseService>();
            services.AddScoped<ICourse, CourseService>();
            services.AddScoped<ICourseSpecialty, CourseSpecialtyService>();
            services.AddScoped<IInstructor, InstructorService>();
            services.AddScoped<IBlog, BlogService>();
            services.AddScoped<IEvent, EventService>();
            services.AddScoped<IFaq, FaqService>();
            services.AddScoped<IGallery, GalleryService>();
            services.AddScoped<ISoical, SoicalService>();
            services.AddScoped<IApplicationIntro, ApplicationIntroService>();
            services.AddScoped<INewsLetter, NewsLetterService>();
            services.AddScoped<IEmailSender, EmailSenderService>();
            services.AddScoped<IHashString, HashStringService>();
            services.AddScoped<ISetting, SettingService>();
            services.AddScoped<IComment, CommentService>();
            services.AddScoped<IBooking, BookingService>();
            services.AddScoped<IUser, UserService>();
            services.AddScoped<ILoginAttemp, LoginAttempService>();
            services.AddScoped<INotification, NotificationService>();

            services.AddAuthorization(op => op.AddPolicy("Admin",
                o => o.RequireRole("Admin").RequireAuthenticatedUser()));

            services.Configure<RequestLocalizationOptions>(options =>
            {
                    var cultures = new List<CultureInfo> {
                            new CultureInfo("en"),
                            new CultureInfo("ar")
                    };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ar");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.Use(async(context, next) => {

                var jwtToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
                }
                await next();
            });

            app.UseStatusCodePages(context => {
                
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.Redirect("/ADCPDA/login");
                }

                return Task.CompletedTask;
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
        // TODO Change the value depending of your needs
        context.Response.Headers.Add("referrer-policy", new StringValues("strict-origin-when-cross-origin"));

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
        context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        context.Response.Headers.Add("x-frame-options", new StringValues("DENY"));

        // https://security.stackexchange.com/questions/166024/does-the-x-permitted-cross-domain-policies-header-have-any-benefit-for-my-websit
        context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
        context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expect-CT
        // You can use https://report-uri.com/ to get notified when a misissued certificate is detected
        context.Response.Headers.Add("Expect-CT", new StringValues("max-age=0, enforce, report-uri=\"https://example.report-uri.com/r/d/ct/enforce\""));

        // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Feature-Policy
        // https://github.com/w3c/webappsec-feature-policy/blob/master/features.md
        // https://developers.google.com/web/updates/2018/06/feature-policy
        // TODO change the value of each rule and check the documentation to see if new features are available
        context.Response.Headers.Add("Feature-Policy", new StringValues(
            "accelerometer 'none';" +
            "ambient-light-sensor 'none';" +
            "autoplay 'none';" +
            "battery 'none';" +
            "camera 'none';" +
            "display-capture 'none';" +
            "document-domain 'none';" +
            "encrypted-media 'none';" +
            "execution-while-not-rendered 'none';" +
            "execution-while-out-of-viewport 'none';" +
            "gyroscope 'none';" +
            "magnetometer 'none';" +
            "microphone 'none';" +
            "midi 'none';" +
            "navigation-override 'none';" +
            "payment 'none';" +
            "picture-in-picture 'none';" +
            "publickey-credentials-get 'none';" +
            "sync-xhr 'none';" +
            "usb 'none';" +
            "wake-lock 'none';" +
            "xr-spatial-tracking 'none';"
            ));

        return _next(context);
    }
}