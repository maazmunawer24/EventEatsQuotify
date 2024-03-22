
using System.Threading.Tasks;

namespace EventEatsQuotify.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
