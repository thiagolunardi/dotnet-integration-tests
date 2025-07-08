using IntegrationTests.Models;

namespace IntegrationTests.Tests.WebApi;

public class DeleteTests : TodoIntegrationTest
{
    [Fact]
    public async Task NotificationEmail_Sent()
    {
        // Arrange
        var itemName = $"To be deleted {Guid.NewGuid()}";
        var newItem = new TodoItem
        {
            Name = itemName
        };
        await AddItems(newItem);

        // Act
        var response = await HttpClient.DeleteAsync($"/api/todo/{newItem.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        await ShouldEventuallyAssert(async () =>
        {
            var messages = await MailpitApi.ListMessages();
            Assert.NotEmpty(messages.Messages);
            Assert.Equal(1, messages.Messages.Count(m => m.Subject.Contains(itemName)));
        });
    }
}