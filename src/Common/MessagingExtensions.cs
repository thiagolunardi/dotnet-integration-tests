using System.Reflection;
using IntegrationTests.Common.Filters;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Common;

public static class MessagingExtensions
{
    public static void AddMessageProducerSettings(this IServiceCollection services)
    {
        services.AddMassTransit(busRegistration => busRegistration.UsingRabbitMq());
    }
    
    public static void AddMessageConsumerSettings(this IServiceCollection services, params Assembly[] consumerAssemblies)
    {
        // services.AddOptions<RabbitMqTransportOptions>(nameof(RabbitMqTransportOptions))
        //     .Bind(hostContext.Configuration.GetSection(nameof(RabbitMqTransportOptions)));
                
        services.AddMassTransit(busRegistration =>
        {
            busRegistration.AddConsumers(consumerAssemblies);
            busRegistration.UsingRabbitMq((busContext, busConfiguration) =>
            {
                busConfiguration.ConfigureEndpoints(busContext);
                busConfiguration.UseConsumeFilter(typeof(UnitOfWorkFilter<>), busContext);
            });
        });
    }
}