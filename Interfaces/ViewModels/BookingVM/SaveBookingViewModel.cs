using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.BookingVM
{
    public class SaveBookingViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Student_name { get; set; }

        [Required]
        public string Phone { get; set; }

        public Guid? Course_id { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
