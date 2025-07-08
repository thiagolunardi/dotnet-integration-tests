// ReSharper disable NotAccessedPositionalProperty.Global
namespace IntegrationTests.Common.Email.Templates;

public abstract record MailTemplate(string Recipient, string Subject, string HtmlContent);

public record ItemCompletedTemplate(string Recipient, string ItemName, long ItemId)
    : MailTemplate(
        Recipient,
        $"Item {ItemName} Completed",
        $"""
        <html>
            <body>
                <h1>Item Completed</h1>
                <p>The item <strong>{ItemName}</strong> with ID <strong>{ItemId}</strong> has been completed.</p>
            </body>
        </html>
        """);
