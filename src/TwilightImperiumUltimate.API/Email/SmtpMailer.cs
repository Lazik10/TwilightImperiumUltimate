using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Web;
using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.API.Email;

public class SmtpMailer(
    IOptions<SmtpOptions> smtpOptions,
    IOptions<FrontendOptions> frontendOptions,
    ILogger<SmtpMailer> logger) : IDisposable
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
    private readonly FrontendOptions _frontendOptions = frontendOptions.Value;
    private readonly ILogger<SmtpMailer> _logger = logger;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task SendConfirmationLinkAsync(TwilightImperiumUser user, string email, string confirmationLink)
    {
        ArgumentNullException.ThrowIfNull(user);

        _logger.LogInformation("Sending confirmation email to {User} with {EmailAddress} email address", user.UserName, email);

        confirmationLink = GetConfirmationEmailBody(confirmationLink);

        await SendEmailAsync(email, "Confirmation Email", confirmationLink);
    }

    public async Task SendPasswordResetCodeAsync(TwilightImperiumUser user, string email, string resetCode)
    {
        ArgumentNullException.ThrowIfNull(user);

        _logger.LogInformation("Sending password reset code to {User} with {EmailAddress} email address", user.UserName, email);

        resetCode = GetResetPasswordEmailBody(resetCode);

        await SendEmailAsync(email, "Password Reset Code", resetCode);
    }

    public async Task SendPasswordResetLinkAsync(TwilightImperiumUser user, string email, string resetLink)
    {
        ArgumentNullException.ThrowIfNull(user);

        _logger.LogInformation("Sending password reset link to {User} with {EmailAddress} email address", user.UserName, email);

        await SendEmailAsync(email, "Password Reset Link", resetLink);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    private Uri CreateConfirmEmailLinkUrl(Uri confirmationLink)
    {
        // Construct the new URL
        var newUrlBase = _frontendOptions.Url;

        var newUrlPath = $"account{confirmationLink.PathAndQuery}";

        // Construct the new Uri using Uri class
        var newUri = new Uri(newUrlBase!, newUrlPath);

        return newUri;
    }

    private Uri CreateResetPasswordEmailLinkUrl(string resetCode)
    {
        var newUrlBase = _frontendOptions.Url;
        var newUrlPath = $"account/password-reset/{resetCode}";
        var newUri = new Uri(newUrlBase!, newUrlPath);

        return newUri;
    }

    private string GetConfirmationEmailBody(string confirmationLink)
    {
        var uri = new Uri(confirmationLink);
        var newConfirmationLink = CreateConfirmEmailLinkUrl(uri);
        return $"<a href=\"{newConfirmationLink}\">Click here to confirm your email</a>";
    }

    private string GetResetPasswordEmailBody(string resetCode)
    {
        var newResetPasswordLink = CreateResetPasswordEmailLinkUrl(resetCode);
        return $"<a href=\"{newResetPasswordLink}\">Click here to reset your password</a>";
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient();

        await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.EnableSsl);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(_smtpOptions.Username, _smtpOptions.Password);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_smtpOptions.Sender, _smtpOptions.SenderEmail));
        message.To.Add(new MailboxAddress(string.Empty, to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        message.Dispose();
    }
}
