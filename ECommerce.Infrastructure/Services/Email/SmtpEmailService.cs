

using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Mail;

namespace ECommerce.Infrastructure.Services.Email
{
    public class SmtpEmailService:IEmailService
    {
        private readonly SmtpOptions _options;
        public SmtpEmailService(IOptions<SmtpOptions> options )
        {
            _options=options.Value;
        }

        public async Task SendAsync(string toEmail,string subject,string body)
        {

            var smtpClient = new SmtpClient(_options.Host,_options.Port)
            {
                
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _options.Username,
                    _options.Password
                    )
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
