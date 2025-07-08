using System.Diagnostics;
using IntegrationTests.MessageProcessor;
using IntegrationTests.Tests.Common.Mailpit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Extensions.AssemblyFixture;
using Xunit.Sdk;

namespace IntegrationTests.Tests;

public abstract class IntegrationTest
    : IClassFixture<WebApplicationFactory<Program>>,
        IClassFixture<MessageProcessorFactory>,
        IAssemblyFixture<TestDatabaseCollectionFixture>,
        IAssemblyFixture<TestMailboxCollectionFixture>,
        IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _webApiFactory = new();
    private readonly IHost _messageProcessorHost = new MessageProcessorFactory().CreateHost();

    protected HttpClient HttpClient = null!;
    protected IServiceProvider WebApiServiceProvider => _webApiFactory.Services;
    protected IServiceProvider MessageProcessorServiceProvider => _messageProcessorHost.Services;

    protected readonly MailpitApi MailpitApi = new();

    public async Task InitializeAsync()
    {
        await _messageProcessorHost.StartAsync();
        HttpClient = new WebApplicationFactory<Program>().CreateClient();
    }

    public async Task DisposeAsync()
    {
        await _messageProcessorHost.StopAsync();
    }

    protected async Task ExecuteInScope<T>(Func<T, Task> action) where T : notnull
    {
        await using var scope = _webApiFactory.Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        await action(service);
    }

    protected async Task<TResult> ExecuteInScope<TService, TResult>(Func<TService, Task<TResult>> action)
        where TService : notnull
    {
        await using var scope = _webApiFactory.Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        return await action(service);
    }

    protected static async Task ShouldEventuallyAssert(Func<Task> assert, TimeSpan? timeout = null,
        TimeSpan? interval = null)
    {
        timeout ??= TimeSpan.FromSeconds(5);
        interval ??= TimeSpan.FromMilliseconds(150);
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                await assert();
            }
            catch (XunitException)
            {
                if (stopwatch.Elapsed > timeout.Value)
                    throw;

                await Task.Delay(interval.Value);
                continue;
            }

            break;
        }
    }
}