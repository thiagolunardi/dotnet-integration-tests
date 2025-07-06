using IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Xunit.Extensions.AssemblyFixture;

namespace IntegrationTests.Tests;

public abstract class TodoIntegrationTest(WebApplicationFactory<Program> factory) : IntegrationTest(factory)
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
    
    protected Task<TodoItem[]> GetTodoItems()
    {
        return ExecuteInScope<TodoContext, TodoItem[]>(ctx=> ctx.TodoItems.ToArrayAsync());
    }
}

public abstract class IntegrationTest : 
    IClassFixture<WebApplicationFactory<Program>>,
    IAssemblyFixture<TestDatabaseCollectionFixture>
{
    private readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient HttpClient;

    protected IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        HttpClient = _factory.CreateClient();
    }
    
    protected async Task ExecuteInScope<T>(Func<T, Task> action) where T : notnull
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        await action(service);
    }
    
    protected async Task<TResult> ExecuteInScope<TService, TResult>(Func<TService, Task<TResult>> action) where TService : notnull
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        return await action(service);
    }
}