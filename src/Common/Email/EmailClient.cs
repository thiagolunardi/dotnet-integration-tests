using System.Net.Mail;
using IntegrationTests.Common.Email.Templates;
using Microsoft.Extensions.Options;

namespace IntegrationTests.Common.Email;

public class EmailClient(IOptions<EmailClientOptions> options) : IEmailClient
{
    public Task SendAsync(MailTemplate template, CancellationToken cancellationToken)
    {
        var smtpClient = new SmtpClient(options.Value.Host, options.Value.Port);
        var mailMessage = new MailMessage(options.Value.Sender, template.Recipient, template.Subject, template.HtmlContent)
        {
            IsBodyHtml = true
        };
        return smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}