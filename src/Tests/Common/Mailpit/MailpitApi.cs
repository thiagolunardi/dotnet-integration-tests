using System.Text.Json.Serialization;
using Flurl.Http;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace IntegrationTests.Tests.Common.Mailpit;

public sealed class MailpitApi
{
    private readonly FlurlClient _flurlClient =
        new($"http://{MailpitOptions.Host}:{MailpitOptions.Port}/api/v1/");

    public async Task<MessageSummary> ListMessages(int start = 0, int limit = 50, CancellationToken cancellationToken = default)
    {
        var summary = await _flurlClient.Request("messages")
            .SetQueryParam("start", start)
            .SetQueryParam("limit", limit)
            .GetJsonAsync<MessageSummary>(cancellationToken: cancellationToken);

        return summary;
    }
    
    public async Task DeleteMessages(params string[] ids)
    {
        await _flurlClient.Request("messages")
            .SendJsonAsync(HttpMethod.Delete, ids);
    }
}

public static class MailpitOptions
{
    public const string Host = "localhost";
    public const int Port = 8025;
}

public sealed class MessageSummary
{
    public required Message[] Messages { get; set; }
    
    [JsonPropertyName("messages_count")]
    public required int MessageCount { get; set; }
    
    [JsonPropertyName("messages_unread")]
    public required int MessagesUnread { get; set; }
    
    public required int Start { get; set; }
    public required string[] Tags { get; set; }
    public required int Total { get; set; }
    public required int Unread { get; set; }
}

public sealed class Message
{
    public required int Attachments { get; set; }
    public required MailAddress[] Bcc { get; set; }
    public required MailAddress[] Cc { get; set; }
    public required DateTime Created { get; set; }
    public required MailAddress From { get; set; }
    public required string Id { get; set; }
    public required string MessageId { get; set; }
    public required bool Read { get; set; }
    public required MailAddress[] ReplyTo { get; set; }
    public required int Size { get; set; }
    public required string Snippet { get; set; }
    public required string Subject { get; set; }
    public required string[] Tags { get; set; }
    public required MailAddress[] To { get; set; }
}

public sealed class MailAddress
{
    public required string Address { get; set; }
    public string Name { get; set; } = string.Empty;
}