using BastilleUserService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp.Authenticators;
using RestSharp;
using System.Net;
using System.Net.Mail;

namespace BastilleUserService.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            var mailMessage = new MailMessage(from,to,subject,body);

            using var client = new SmtpClient(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            };
            await client.SendMailAsync(mailMessage);
        }

        public RestResponse SendEmail(string body, string email)
        {
            var clientoptions = new RestClientOptions();
            clientoptions.BaseUrl = new Uri("https://api.mailgun.net/v3");
            clientoptions.Authenticator = new HttpBasicAuthenticator("api", "a535556ff2667ea48bbf621528d93436-db4df449-e33a633f");
            var request = new RestRequest(resource: "", Method.Post);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("authorization", $"api : {clientoptions.Authenticator}");
            request.AddParameter("domain", "sandboxbf28a03222424239bfbd765131f93e52.mailgun.org",ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Excited User <mailguncf@sandboxbf28a03222424239bfbd765131f93e52.mailgun.org>");
            request.AddParameter("to", email);
            request.AddParameter("subject", "Email Verfifvation");
            request.AddParameter("text", body);
            request.Method = Method.Post;
            var client = new RestClient(clientoptions);
            return client.Execute(request);
        }

    }
}
