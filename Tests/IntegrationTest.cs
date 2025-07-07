using IntegrationTests.MessageProcessor;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Extensions.AssemblyFixture;

namespace IntegrationTests.Tests;

public abstract class IntegrationTest
    : IClassFixture<WebApplicationFactory<Program>>,
        IClassFixture<MessageProcessorFactory>,
        IAssemblyFixture<TestDatabaseCollectionFixture>,
        IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _webApiFactory = new();
    private readonly IHost _messageProcessorHost = new MessageProcessorFactory().CreateHost();

    protected HttpClient HttpClient = null!;
    protected IServiceProvider WebApiServiceProvider => _webApiFactory.Services;
    protected IServiceProvider MessageProcessorServiceProvider => _messageProcessorHost.Services;

    public async Task InitializeAsync()
    {
        await _messageProcessorHost.StartAsync();
        HttpClient = new WebApplicationFactory<Program>().CreateClient();
    }

    public async Task DisposeAsync()
    {
        await _messageProcessorHost.StopAsync();
        await _webApiFactory.DisposeAsync();
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
}