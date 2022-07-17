using Entitis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.CourseVM
{
    public class GetCousrseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Course_details { get; set; }
        public string Image { get; set; }
        public int Rate { get; set; }
        public float Price { get; set; }
        public float Price_after_discount => (Discount_value != 0) ? (Price * Discount_value)/100 : 0;
        public int Discount_value { get; set; }
        public bool Is_active { get; set; }

        public Course_specialty Course_specialty { get; set; }
        public Instructors Instructors { get; set; }
    }
}
