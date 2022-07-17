using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.NewsLetterVM
{
    public class GetNewsLetterViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool Is_active { get; set; }

    }
}
