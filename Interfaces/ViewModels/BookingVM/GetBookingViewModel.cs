using Entitis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.BookingVM
{
    public class GetBookingViewModel
    {
        public Guid Id { get; set; }
        public string Student_name { get; set; }
        public string Phone { get; set; }
        public Courses Courses { get; set; }
        public bool Is_active { get; set; }

    }
}
