using IntegrationTests.Models;
using Microsoft.EntityFrameworkCore;

namespace Models;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }
}