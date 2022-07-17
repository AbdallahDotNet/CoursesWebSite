using Interfaces.Helper;
using Interfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Courses.Services
{
    public class EmailSenderService : IEmailSender
    {
        public void SendEmailAsync(MailRequest mail_request, MailSetting mail_setting)
        {
            var mail_account = mail_setting.Mail.Split('@','.')[1];

            if (mail_account == "gmail" || mail_account == "outlook")
            {
                mail_setting.Host = (mail_account == "gmail") ? "smtp.gmail.com" : "smtp.live.com";
                mail_setting.Port = 587;
            }
            else
            {
                mail_setting.Host = (mail_account == "yahoo") ? "smtp.mail.yahoo.com" : "smtp.live.com";
                mail_setting.Port = 465;
            }

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress(mail_setting.Mail); //IMPORTANT: This must be same as your smtp authentication address.
            mail.To.Add(mail_request.To_email);

            //set the content 
            mail.Subject = mail_request.Subject;
            mail.Body = mail_request.Body;
            //send the message 
            SmtpClient smtp = new SmtpClient(mail_setting.Host);

            //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
            NetworkCredential Credentials = new NetworkCredential(mail_setting.Mail, mail_setting.Password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = Credentials;
            smtp.Port = mail_setting.Port;    //alternative port number is 8889
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
