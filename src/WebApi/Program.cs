using System.Reflection;
using IntegrationTests.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
builder.Configuration
    .SetBasePath(basePath)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddJsonFile("appsettings.Test.json", optional: true);

builder.Services.AddDbContext<TodoContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOptions<RabbitMqTransportOptions>(nameof(RabbitMqTransportOptions))
    .Bind(builder.Configuration.GetSection(nameof(RabbitMqTransportOptions)))
    .ValidateDataAnnotations();

builder.Services.AddMassTransit(busRegistration => busRegistration.UsingRabbitMq());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Ingestion middleware
app.Use(async (context, next) =>
{
    Console.WriteLine("Ingestion: Processing request " + context.Request.Path);
    await next();
});

// Response middleware (executes after endpoint processing)
app.Use(async (context, next) =>
{
    await next();
    Console.WriteLine("Return: Processed response with status code " + context.Response.StatusCode);
});

app.UseHttpsRedirection();

app.MapGet("/api/todo", async (TodoContext context) =>
    await context.TodoItems.ToListAsync());

app.MapGet("/api/todo/{id}", async (long id, TodoContext context) =>
{
    var todoItem = await context.TodoItems.FindAsync(id);
    return todoItem is not null ? Results.Ok(todoItem) : Results.NotFound();
});

app.MapPost("/api/todo", async (TodoItem todoItem, TodoContext context) =>
{
    context.TodoItems.Add(todoItem);
    await context.SaveChangesAsync();
    return Results.Created($"/api/todo/{todoItem.Id}", todoItem);
});

app.MapPut("/api/todo/{id}", async (long id, TodoItem todoItem, TodoContext context, IBus bus) =>
{
    if (id != todoItem.Id)
        return Results.BadRequest();

    // Publish an event to the message bus
    var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3));
    await bus.Publish(new MarkAsCompletedCommand(todoItem.Id), cancellationToken.Token);

    return Results.NoContent();
});

app.MapDelete("/api/todo/{id}", async (long id, TodoContext context) =>
{
    var todoItem = await context.TodoItems.FindAsync(id);
    if (todoItem == null)
        return Results.NotFound();

    context.TodoItems.Remove(todoItem);
    await context.SaveChangesAsync();

    return Results.Ok(todoItem);
});

app.Run();

public partial class Program { }