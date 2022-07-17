using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.EventVM
{
    public class GetEventViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public int Timing { get; set; }
        public string Content { get; set; }
        public string Address { get; set; }
        public bool Is_active { get; set; }

    }
}
