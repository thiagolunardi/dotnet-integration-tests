using IntegrationTests.Common.Email.Templates;

namespace IntegrationTests.Common.Email;

public interface IEmailClient
{
    Task SendAsync(MailTemplate template, CancellationToken cancellationToken);
}