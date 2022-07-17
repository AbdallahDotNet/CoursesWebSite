using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Helper
{
    public class MailRequest
    {
        public string To_email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
