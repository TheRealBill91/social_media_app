using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SocialMediaApp.Services;

public class EmailSender : IEmailSender
{
    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("bc357gaming@gmail.com", "Account confirmation"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        Console.WriteLine(response);
    }
}
