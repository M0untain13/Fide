﻿using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Fide.Blazor.Services.EmailSender;

public class SmtpEmailSender(string host, int port, string email, string password) : IEmailSender
{
    public Task SendEmailAsync(string recipientEmail, string subject, string message)
    {
        using var client = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(email, password),
            EnableSsl = true,
        };

        return client.SendMailAsync(
            new MailMessage(
                from: email,
                to: recipientEmail,
                subject,
                message
            )
        );
    }
}
