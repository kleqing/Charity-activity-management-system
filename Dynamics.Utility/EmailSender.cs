using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace Dynamics.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Because we're using app password, mail should be sent from me (Who register it in google)
            // Also, if email does not exist, the mail will be redirected into my mail box
            try
            {
                string fromMail = "kietpmde180889@fpt.edu.vn";
                string fromPassword = @"pucw aypg zxws tjcb";
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // SMTP server
                    Port = 587, // TLS port
                    EnableSsl = true, // Enable TLS (SSL)
                    Credentials = new System.Net.NetworkCredential(fromMail, fromPassword) // MY Username and password
                };

                MailMessage mail = new MailMessage();
                mail.Subject = subject;
                //mail.Subject = "Confirmation Email from Dynamics.com";
                mail.Body = htmlMessage.ToString();
                //Setting From , To and CC
                mail.From = new MailAddress(fromMail);
                mail.To.Add(new MailAddress(email));
                //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));
                mail.IsBodyHtml = true; // here
                smtpClient.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Task.CompletedTask;
        }
    }
}
