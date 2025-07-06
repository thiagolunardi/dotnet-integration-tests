using System.Net.Http.Json;
using IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Tests.TodoTests;

public class UpdateTests(WebApplicationFactory<Program> factory) : TodoIntegrationTest(factory)
{
    [Fact]
    public async Task UpdateStatus_SaveAsCompleted()
    {
        // Arrange
        var itemName = $"Item to update {Guid.NewGuid()}";
        var newItem = new TodoItem
        {
            Name = itemName
        };
        await AddItems(newItem);

        // Act
        newItem.IsComplete = true;
        var response = await HttpClient.PutAsJsonAsync($"/api/todo/{newItem.Id}", newItem);

        // Assert
        response.EnsureSuccessStatusCode();

        var todoItems = await GetTodoItems();
        var updatedItem = todoItems.SingleOrDefault(item => item.Id == newItem.Id);
        Assert.NotNull(updatedItem);
        Assert.Equal(itemName, updatedItem.Name);
        Assert.True(updatedItem.IsComplete);
    }
}