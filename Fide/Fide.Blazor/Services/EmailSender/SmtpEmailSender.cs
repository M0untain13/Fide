using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Fide.Blazor.Services.EmailSender;

public class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender
{
    private readonly SmtpOptions _smtpOptions = options.Value;

    public async Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        using var client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port)
        {
            Credentials = new NetworkCredential(_smtpOptions.Email, _smtpOptions.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage(
            from: _smtpOptions.Email,
            to: recipientEmail,
            subject,
            message
        )
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(mailMessage);
    }
}
