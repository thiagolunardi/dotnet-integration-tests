using IntegrationTests.Common.Email;
using IntegrationTests.Common.Email.Templates;
using IntegrationTests.Common.Messaging;
using IntegrationTests.Common.Settings;
using IntegrationTests.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

builder.Configuration.AddAppSettings();

builder.Services.AddTodoContext(builder.Configuration);
builder.Services.AddMessageProducerSettings();
builder.Services.AddEmailClient(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/todo", async (TodoContext context) =>
    await context.TodoItems.ToListAsync());

app.MapGet("/api/todo/{id:long}", async (long id, TodoContext context) =>
{
    var todoItem = await context.TodoItems.FindAsync(id);
    return todoItem is not null ? Results.Ok(todoItem) : Results.NotFound();
});

app.MapPost("/api/todo", (TodoItem todoItem, TodoContext context) =>
{
    context.TodoItems.Add(todoItem);
    
    return Task.FromResult(Results.Created($"/api/todo/{todoItem.Id}", todoItem));
});

app.MapPut("/api/todo/{id:long}", async (long id, TodoItem todoItem, IBus bus, CancellationToken cancellationToken) =>
{
    if (id != todoItem.Id)
        return Results.BadRequest();
    
    await bus.Publish(new MarkAsCompletedCommand(todoItem.Id), cancellationToken);

    return Results.NoContent();
});

app.MapDelete("/api/todo/{id:long}", async (long id, TodoContext context, IEmailClient email, CancellationToken cancellationToken) =>
{
    var todoItem = await context.TodoItems.FindAsync(id);
    if (todoItem == null)
        return Results.NotFound();

    context.TodoItems.Remove(todoItem);
    
    var mailTemplate = new ItemCompletedTemplate("user@emailaddress.com", todoItem.Name, todoItem.Id);
    await email.SendAsync(mailTemplate, cancellationToken);

    return Results.Ok(todoItem);
});

app.Use(async (context, next) =>
{
    await next();
    
    if (context.Request.Method == HttpMethods.Get)
    {
        return;
    }

    if (context.Response.StatusCode is >= 200 and <= 299 or 302)
    {
        var cancellationToken = context.Request.HttpContext.RequestAborted;
        var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
});

app.Run();

public partial class Program;
