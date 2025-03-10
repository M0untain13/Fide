using Fide.Blazor.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Fide.Blazor.Services.EmailSender;

public class ApplicationUserEmailSender(IEmailSender emailSender) : IEmailSender<ApplicationUser>
{
    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        await emailSender.SendEmailAsync(
            email, 
            "Потвердите почту", 
            $"Чтобы завершить регистрацию аккаунта <a href='{confirmationLink}'>нажмите здесь</a>."
        );
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        await emailSender.SendEmailAsync(
            email, 
            "Сброс пароля", 
            $"Скопируйте код для сброса пароля: {resetCode}"
        );
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        await emailSender.SendEmailAsync(
            email, 
            "Сброс пароля", 
            $"Чтобы сбросить пароль <a href='{resetLink}'>нажмите здесь</a>."
        );
    }
}
