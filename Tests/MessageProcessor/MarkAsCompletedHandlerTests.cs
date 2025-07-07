using System.Diagnostics;
using IntegrationTests.Contracts;
using IntegrationTests.Models;
using Xunit.Sdk;

namespace IntegrationTests.Tests.MessageProcessor;

public class MarkAsCompletedHandlerTests : TodoIntegrationTest
{
    [Fact]
    public async Task TodoItemCompletedEventHandler_ProcessesEvent()
    {
        // Arrange
        var itemName = $"Item to complete {Guid.NewGuid()}";
        var newItem = new TodoItem
        {
            Name = itemName
        };
        await AddItems(newItem);

        // Act
        var message = new MarkAsCompletedCommand(newItem.Id);
        await Publish(message);

        // Assert
        var timeout = TimeSpan.FromSeconds(5);
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            try
            {
                var updatedItem = await GetTodoItem(message.Id);
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
