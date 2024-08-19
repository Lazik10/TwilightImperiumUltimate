using Microsoft.AspNetCore.Identity;
using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.API.Email;

public class IdentityEmailSender(SmtpMailer smtpMailer) : IEmailSender<TwilightImperiumUser>
{
    public async Task SendConfirmationLinkAsync(TwilightImperiumUser user, string email, string confirmationLink)
    {
        await smtpMailer.SendConfirmationLinkAsync(user, email, confirmationLink);
    }

    public async Task SendPasswordResetCodeAsync(TwilightImperiumUser user, string email, string resetCode)
    {
        await smtpMailer.SendPasswordResetCodeAsync(user, email, resetCode);
    }

    public async Task SendPasswordResetLinkAsync(TwilightImperiumUser user, string email, string resetLink)
    {
        await smtpMailer.SendPasswordResetLinkAsync(user, email, resetLink);
    }
}
