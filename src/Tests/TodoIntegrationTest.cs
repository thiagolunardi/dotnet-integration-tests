using IntegrationTests.Contracts;
using IntegrationTests.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Tests;

public abstract class TodoIntegrationTest : IntegrationTest
{
    protected async Task AddItems(params TodoItem[] todoItems)
    {
        await ExecuteInScope<TodoContext>(async ctx =>
        {
            foreach (var todoItem in todoItems)
            {
                ctx.TodoItems.Add(todoItem);
            }

            await ctx.SaveChangesAsync();
        });
    }
    
    protected Task<TodoItem?> GetTodoItem(long id)
    {
        return ExecuteInScope<TodoContext, TodoItem?>(ctx=> 
            ctx.TodoItems.FirstOrDefaultAsync(item => item.Id == id));
    }

    protected Task<TodoItem?> GetTodoItem(string name)
    {
        return ExecuteInScope<TodoContext, TodoItem?>(ctx=> 
            ctx.TodoItems.FirstOrDefaultAsync(item => item.Name == name));
    }

    protected Task Publish(MarkAsCompletedCommand message)
    {
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        return ExecuteInScope<IBus>(async bus =>
            await bus.Publish(message, cancellationToken.Token));
    }
}