// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace IntegrationTests.Models;

public class TodoItem
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public bool IsComplete { get; set; }
}