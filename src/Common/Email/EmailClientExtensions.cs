using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Common.Email;

public static class EmailClientExtensions
{
    public static IServiceCollection AddEmailClient(this IServiceCollection services, IConfiguration configuration)
    {
         services
             .AddOptions<EmailClientOptions>()
             .Bind(configuration.GetSection(nameof(EmailClientOptions)))
             .ValidateOnStart();
         
         configuration.Bind(configuration.GetSection(nameof(EmailClientOptions)));
        
        services.AddSingleton<IEmailClient, EmailClient>();
        return services;
    }
}