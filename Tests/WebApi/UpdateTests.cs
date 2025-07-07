using System.Diagnostics;
using System.Net.Http.Json;
using IntegrationTests.Models;
using Xunit.Sdk;

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
        
        var timeout = TimeSpan.FromSeconds(5);
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                var updatedItem = await GetTodoItem(newItem.Id);
                Assert.NotNull(updatedItem);
                Assert.Equal(itemName, updatedItem.Name);
                Assert.True(updatedItem.IsComplete);
            }
            catch (XunitException)
            {
                if (stopwatch.Elapsed > timeout)
                    throw;
                
                await Task.Delay(TimeSpan.FromMilliseconds(150)); // Wait for message processing
                continue;
            }
            break;
        }
    }
}