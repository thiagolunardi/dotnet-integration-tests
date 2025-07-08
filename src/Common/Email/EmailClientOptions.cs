// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace IntegrationTests.Common.Email;

public sealed class EmailClientOptions
{
    public required string Sender { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
}