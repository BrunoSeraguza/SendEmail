using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using blogapi.Controller;
using Microsoft.AspNetCore.Mvc;

namespace blogapi.Service
{
    public class EmailService
    {
        public bool SendEmail(
            string toName,
            string toEmail,
            string subject,
            string body,
            string fromName = "Bruno .NET",
            string fromEmail = "bruno.seraguza@softlinesistemas.com.br"
        )
        {
            // var smpt = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port)
            // {
            //     Credentials = new NetworkCredential(Configuration.Smtp.Name, Configuration.Smtp.Password),
            //     DeliveryMethod = SmtpDeliveryMethod.Network,
            //     EnableSsl = true
            // };

             var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
            
               smtpClient. Credentials = new NetworkCredential(Configuration.Smtp.Name, Configuration.Smtp.Password);
               smtpClient. DeliveryMethod = SmtpDeliveryMethod.Network;
               smtpClient. EnableSsl = true;
            


            var mail = new MailMessage()
            {
                From = new MailAddress(fromEmail, fromName),
                Body = body,
                Subject = subject,
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress(toEmail, toName));
            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine( ex  );
                return false;
            }

        }
    }
}