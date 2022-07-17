using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitis.Models
{
    public class Login_attempts
    {
        public Guid Id { get; set; }
        public string Ip_address { get; set; }
        public int Attemps { get; set; }
        public DateTime Date { get; set; }
        public bool Blocked { get; set; }
        public bool Is_active { get; set; }
    }
}
