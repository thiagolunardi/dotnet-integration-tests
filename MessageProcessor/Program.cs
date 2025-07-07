using IntegrationTests.MessageProcessor;
using Microsoft.Extensions.Hosting;

var host = new MessageProcessorFactory().CreateHost();

await host.RunAsync();