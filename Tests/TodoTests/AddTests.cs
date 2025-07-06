using System.Net.Http.Json;
using IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Tests.TodoTests;

public class AddTests(WebApplicationFactory<Program> factory)
    : TodoIntegrationTest(factory)
{
    [Fact]
    public async Task AddItem_Success()
    {
        // Arrange
        var itemName = $"Recently added Item {Guid.NewGuid()}";
        var newItem = new TodoItem
        {
            Name = itemName
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/todo", newItem);

        // Assert
        response.EnsureSuccessStatusCode();

        var todoItems = await GetTodoItems();
        var addedItem = todoItems.SingleOrDefault(item => item.Name == itemName);
        Assert.NotNull(addedItem);
        Assert.Equal(itemName, addedItem.Name);
        Assert.False(addedItem.IsComplete);
    }
}