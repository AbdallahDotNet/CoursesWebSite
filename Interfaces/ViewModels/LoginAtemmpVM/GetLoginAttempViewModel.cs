using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.ViewModels.LoginAtemmpVM
{
    public class GetLoginAttempViewModel
    {
        public Guid Id { get; set; }
        public string Ip_address { get; set; }
        public int Attemps { get; set; }
        public string Date { get; set; }
        public bool Blocked { get; set; }
    }
}
