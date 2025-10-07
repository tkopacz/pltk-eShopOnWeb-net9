#if NETCOREAPP2_0

using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.Infrastructure.Services
{
    public class LoggerEmailSender : IEmailSender
    {
        private readonly IAppLogger<LoggerEmailSender> _logger;

        public LoggerEmailSender(IAppLogger<LoggerEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            _logger.LogInformation("to: {email}, subject: {emailSubject}, message: {emailMessage}", email, subject, message);
            return Task.CompletedTask;
        }
    }
}

#endif
