using System.Net.Http.Json;
using IntegrationTests.Models;

namespace IntegrationTests.Tests.WebApi;

public class UpdateTests : TodoIntegrationTest
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
        await ShouldEventuallyAssert(async () =>
        {
            var updatedItem = await GetTodoItem(newItem.Id);
            Assert.NotNull(updatedItem);
            Assert.Equal(itemName, updatedItem.Name);
            Assert.True(updatedItem.IsComplete);
        });
    }
}