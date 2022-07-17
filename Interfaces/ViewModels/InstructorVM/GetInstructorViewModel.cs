using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.InstructorVM
{
    public class GetInstructorViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Short_description { get; set; }
        public string Specialty { get; set; }
        public string Image { get; set; }
        public int Rate { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public bool Is_active { get; set; }

    }
}
