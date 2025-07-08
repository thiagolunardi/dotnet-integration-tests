using MassTransit;
using IntegrationTests.Models;

namespace IntegrationTests.MessageProcessor.Filters;

public class UnitOfWorkFilter<T> 
    : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        await next.Send(context);
        var unitOfWork = context.GetServiceOrCreateInstance<IUnitOfWork>();
        if (unitOfWork != null)
        {
            await unitOfWork.SaveChangesAsync(context.CancellationToken);
        }
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("unitOfWork");
    }
}

