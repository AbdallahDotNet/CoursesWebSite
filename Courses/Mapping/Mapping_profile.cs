using AutoMapper;
using Entitis.Models;
using Interfaces.ViewModels.ApplicationIntroVM;
using Interfaces.ViewModels.BlogVM;
using Interfaces.ViewModels.BookingVM;
using Interfaces.ViewModels.CommentVM;
using Interfaces.ViewModels.CourseSpecialtyVM;
using Interfaces.ViewModels.CourseVM;
using Interfaces.ViewModels.EventVM;
using Interfaces.ViewModels.FaqVM;
using Interfaces.ViewModels.GalleryVM;
using Interfaces.ViewModels.InstructorVM;
using Interfaces.ViewModels.LoginAtemmpVM;
using Interfaces.ViewModels.NewsLetterVM;
using Interfaces.ViewModels.NotificationVM;
using Interfaces.ViewModels.NotifySettingVM;
using Interfaces.ViewModels.SoicalVM;
using Interfaces.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Mapping
{
    public class Mapping_profile : Profile
    {
        public Mapping_profile()
        {
            CreateMap<Entitis.Models.Courses, GetCousrseViewModel>();

            CreateMap<SaveCourseViewModel, Entitis.Models.Courses>().ReverseMap();

            CreateMap<Instructors, GetInstructorViewModel>();
            CreateMap<SaveInstructorViewModel, Instructors>().ReverseMap();

            CreateMap<Course_specialty, GetCourseSpecialtyViewModel>();
            CreateMap<SaveCourseSpecialtyViewModel, Course_specialty>().ReverseMap();

            CreateMap<Blogs, GetBlogViewModel>();
            CreateMap<SaveBlogViewModel, Blogs>().ReverseMap();

            CreateMap<Events, GetEventViewModel>();
            CreateMap<SaveEventViewModel, Events>().ReverseMap();

            CreateMap<Faqs, GetFaqViewModel>();
            CreateMap<SaveFaqViewModel, Faqs>().ReverseMap();

            CreateMap<Gallery, GetGalleryViewModel>();
            CreateMap<SaveGalleryViewModel, Gallery>().ReverseMap();

            CreateMap<Soical, GetSoicalViewModel>();
            CreateMap<SaveSoicalViewModel, Soical>().ReverseMap();

            CreateMap<Application_intro, GetApplicationIntroViewModel>();
            CreateMap<SaveApplicationIntroViewModel, Application_intro>().ReverseMap();

            CreateMap<NewsLetter, GetNewsLetterViewModel>();
            CreateMap<SaveNewsLetterViewModel, NewsLetter>();

            CreateMap<Setting, GetSettingViewModel>();
            CreateMap<SaveSettingViewModel, Setting>().ReverseMap();

            CreateMap<Comments, GetCommentViewModel>();
            CreateMap<SaveCommentViewModel, Comments>();

            CreateMap<Booking, GetBookingViewModel>();
            CreateMap<SaveBookingViewModel, Booking>();

            CreateMap<Users, GetUserViewModel>()
                .ForMember(x => x.Name, op => op.MapFrom(o => o.Name));
            CreateMap<SaveUserViewModel, Users>()
                .ReverseMap()
                .ForMember(x => x.Name, op => op.MapFrom(o => o.Name))
                .ForMember(x => x.Password, op => op.MapFrom(o => o.PasswordHash));

            CreateMap<Login_attempts, GetLoginAttempViewModel>();

            CreateMap<Notifications, GetNotificationViewModel>();
            CreateMap<SaveNotificationViewModel, Notifications>();
        }
    }
}
