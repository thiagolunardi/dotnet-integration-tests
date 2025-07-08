using System.Reflection;
using IntegrationTests.MessageProcessor.Filters;
using IntegrationTests.MessageProcessor.Handlers;
using IntegrationTests.Models;
using MassTransit;
using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests.MessageProcessor;

public class MessageProcessorFactory
{
    private readonly IHostBuilder _hostBuilder;

    public MessageProcessorFactory()
    {
        _hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(config =>
            {
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
                config
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile("appsettings.Development.json", optional: true)
                    .AddJsonFile("appsettings.Test.json", optional: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTodoContext(hostContext.Configuration);
                
                services.AddOptions<RabbitMqTransportOptions>(nameof(RabbitMqTransportOptions))
                    .Bind(hostContext.Configuration.GetSection(nameof(RabbitMqTransportOptions)));
                
                services.AddMassTransit(busRegistration =>
                {
                    busRegistration.AddConsumer<MarkAsCompletedHandler>();
                    busRegistration.UsingRabbitMq((busContext, busConfiguration) =>
                    {
                        busConfiguration.ConfigureEndpoints(busContext);
                        busConfiguration.UseConsumeFilter(typeof(UnitOfWorkFilter<>), busContext);
                    });
                });
            });
    }
    
    public IHost CreateHost() => _hostBuilder.Build();
}
