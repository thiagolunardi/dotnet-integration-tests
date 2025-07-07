using IntegrationTests.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Models;

namespace IntegrationTests.MessageProcessor.Handlers;

// ReSharper disable once ClassNeverInstantiated.Global
public class MarkAsCompletedHandler(TodoContext todoContext) : IConsumer<MarkAsCompletedCommand>
{
    public async Task Consume(ConsumeContext<MarkAsCompletedCommand> context)
    {
        var todoItem = await todoContext.TodoItems.FirstOrDefaultAsync(item => item.Id == context.Message.Id);
        if (todoItem is null) return;

        todoItem.IsComplete = true;
        todoContext.TodoItems.Update(todoItem);

        await todoContext.SaveChangesAsync();
    }
}