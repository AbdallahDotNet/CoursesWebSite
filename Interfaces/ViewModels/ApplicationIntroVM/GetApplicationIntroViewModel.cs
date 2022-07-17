using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.ApplicationIntroVM
{
    public class GetApplicationIntroViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string About { get; set; }
        public string Logo { get; set; }
        public string Coin { get; set; }
        public bool Is_active { get; set; }
    }
}
