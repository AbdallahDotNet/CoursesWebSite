using Interfaces.Helper;
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
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels
{
    public class ContainerViewModels
    {
        public Guid Course_id { get; set; }
        public GetApplicationIntroViewModel Get_application_intro { get; set; }
        public SaveApplicationIntroViewModel Save_application_intro { get; set; }

        public PagedList<GetBlogViewModel> Get_blogs_pagination { get; set; }
        public List<GetBlogViewModel> Get_blogs { get; set; }
        public SaveBlogViewModel Save_blog { get; set; }
        public SearchBlogViewModel Search_blog { get; set; }

        public List<GetBookingViewModel> Get_bookings { get; set; }
        public SaveBookingViewModel Save_booking { get; set; }

        public List<GetCommentViewModel> Get_comments { get; set; }
        public SaveCommentViewModel Save_comment { get; set; }

        public PagedList<GetCousrseViewModel> Get_cousrses_pagination { get; set; }
        public List<GetCousrseViewModel> Get_cousrses { get; set; }
        public SaveCourseViewModel Save_course { get; set; }
        public SearchCourseViewModel Search_course { get; set; }

        public List<GetCourseSpecialtyViewModel> Get_course_specialtys { get; set; }
        public SaveCourseSpecialtyViewModel Save_course_specialty { get; set; }

        public MailRequest Mail_request { get; set; }

        public List<GetEventViewModel> Get_events { get; set; }
        public SaveEventViewModel Save_event { get; set; }

        public List<GetFaqViewModel> Get_faqs { get; set; }
        public SaveFaqViewModel Save_faq { get; set; }

        public List<GetGalleryViewModel> Get_galleries { get; set; }
        public SaveGalleryViewModel Save_gallery { get; set; }

        public List<GetInstructorViewModel> Get_instructors { get; set; }
        public SaveInstructorViewModel Save_instructor { get; set; }

        public List<GetLoginAttempViewModel> Get_login_attemps { get; set; }

        public List<GetNewsLetterViewModel> Get_news_letters { get; set; }
        public SaveNewsLetterViewModel Save_news_letter { get; set; }

        public GetSettingViewModel Get_setting { get; set; }
        public SaveSettingViewModel Save_setting { get; set; }

        public GetSoicalViewModel Get_soical { get; set; }
        public SaveSoicalViewModel Save_soical { get; set; }

        public List<GetUserViewModel> Get_users { get; set; }
        public SaveUserViewModel Save_user { get; set; }

        public List<GetNotificationViewModel> Get_notifications { get; set; }
    }
}
