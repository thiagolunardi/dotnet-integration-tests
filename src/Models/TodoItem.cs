namespace IntegrationTests.Models;

public class TodoItem
{
    public long Id { get; init; } = 0;
    public required string Name { get; set; }
    public bool IsComplete { get; set; } = false;
}