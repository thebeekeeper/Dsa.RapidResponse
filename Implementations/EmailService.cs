
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Specialized;
using SendGrid;
using SendGrid.Helpers.Mail;
using Dsa.RapidResponse.Services;

namespace Dsa.RapidResponse.Implementations
{
    public class EmailService : IEmailService
    {
        async void IEmailService.Send(string to, string message)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("milwaukeedsa@gmail.com", "DSA Milwaukee"));

            var recipients = new List<EmailAddress>
            {
                new EmailAddress(to),
            };
            msg.AddTos(recipients);

            msg.SetSubject("Rapid Response Message");

            msg.AddContent(MimeType.Text, message);
            msg.AddContent(MimeType.Html, message);

            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(msg);
        }
    }
}