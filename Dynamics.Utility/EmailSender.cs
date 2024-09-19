using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace Dynamics.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Because we're using app password, any mail send should always direct to the email with password
            // Therefore i will always send it to my self
            try
            {
                string fromMail = "kietpmde180889@fpt.edu.vn";
                string fromPassword = @"pucw aypg zxws tjcb";
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // SMTP server
                    Port = 587, // TLS port
                    EnableSsl = true, // Enable TLS (SSL)
                    Credentials = new System.Net.NetworkCredential(fromMail, fromPassword) // Username and password
                };

                MailMessage mail = new MailMessage();
                mail.Subject = "Confirmation Email from Dynamics.com";
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
