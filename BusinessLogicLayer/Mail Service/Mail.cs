using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mail_Service
{
    public class Email
    {
        public async static Task SendMailAsync(string subject,string body, string destination)
        {
            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(destination));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            //Attempting to send the email
            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
