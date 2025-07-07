using IntegrationTests.Contracts;
using IntegrationTests.Models;

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
        await ShouldEventuallyAssert(async () =>
        {
            var updatedItem = await GetTodoItem(message.Id);
            Assert.NotNull(updatedItem);
            Assert.Equal(itemName, updatedItem.Name);
            Assert.True(updatedItem.IsComplete);
        });
    }
}
