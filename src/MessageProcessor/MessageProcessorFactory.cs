using System.Reflection;
using IntegrationTests.Common.Messaging;
using IntegrationTests.Common.Settings;
using IntegrationTests.Models;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests.MessageProcessor;

public class MessageProcessorFactory
{
    private readonly IHostBuilder _hostBuilder;

    public MessageProcessorFactory()
    {
        _hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configuration => configuration.AddAppSettings())
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTodoContext(hostContext.Configuration);
                services.AddMessageConsumerSettings(Assembly.GetAssembly(typeof(MessageProcessorFactory))!);
            });
    }
    
    public IHost CreateHost() => _hostBuilder.Build();
}
