using RestSharp;

namespace BastilleUserService.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string from, string to, string subject, string body);
        RestResponse SendEmail(string body, string email);
    }
}