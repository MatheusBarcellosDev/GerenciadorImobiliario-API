using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Blog.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Send
            (
                string toName,
                string toEmail,
                string subject,
                string body,
                string fromName = "Matheus",
                string fromEmail = "emailteste@lorenalorenzoimoveis.com.br"
            )
        {
            var smtpClient = new SmtpClient(
                _configuration["Smtp:Host"],
                int.Parse(_configuration["Smtp:Port"])
            );

            smtpClient.Credentials = new NetworkCredential(
                _configuration["Smtp:Username"],
                _configuration["Smtp:Password"]
            );
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(toEmail, toName));

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
