using System.Net.Http.Json;
using IntegrationTests.Models;

namespace IntegrationTests.Tests.WebApi;

public class AddTests : TodoIntegrationTest
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

        var addedItem = await GetTodoItem(itemName);
        Assert.NotNull(addedItem);
        Assert.Equal(itemName, addedItem.Name);
        Assert.False(addedItem.IsComplete);
    }
}