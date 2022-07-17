using Interfaces.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IEmailSender
    {
        void SendEmailAsync(MailRequest mail_request, MailSetting mail_setting);
    }
}
