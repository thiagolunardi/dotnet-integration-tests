using System.Net.Http.Json;
using IntegrationTests.Models;

namespace IntegrationTests.Tests.WebApi;

public class ListTests : TodoIntegrationTest
{
    [Fact]
    public async Task NewItem_NotCompleted()
    {
        // Arrange
        var newItemName = $"New Item {Guid.NewGuid()}";
        await AddItems(new TodoItem
        {
            Name = newItemName
        });
        
        // Act
        var response = await HttpClient.GetAsync("/api/todo");
        var todoList = await response.Content.ReadFromJsonAsync<List<TodoItem>>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(todoList);
        
        var newItem = await GetTodoItem(newItemName);
        Assert.NotNull(newItem);
        Assert.Equal(newItemName, newItem.Name);
        Assert.False(newItem.IsComplete);
    }
    
    [Fact]
    public async Task TwoItems_CorrectStatus()
    {
        // Arrange
        var incompleteItemName = $"Incomplete Item {Guid.NewGuid()}";
        var completedItemName = $"Completed Item {Guid.NewGuid()}";
        await AddItems(new TodoItem
        {
            Name = incompleteItemName
        }, new TodoItem
        {
            Name = completedItemName,
            IsComplete = true
        });
        
        // Act
        var response = await HttpClient.GetAsync("/api/todo");
        var todoList = await response.Content.ReadFromJsonAsync<List<TodoItem>>();

        // Assert
        response.EnsureSuccessStatusCode();

        Assert.NotNull(todoList);
        Assert.NotEmpty(todoList);
        
        var incompleteItem = todoList.FirstOrDefault(item => item.Name == incompleteItemName);
        Assert.NotNull(incompleteItem);
        Assert.Equal(incompleteItemName, incompleteItem.Name);
        Assert.False(incompleteItem.IsComplete);

        var completedItem = todoList.FirstOrDefault(item => item.Name == completedItemName);
        Assert.NotNull(completedItem);
        Assert.Equal(completedItemName, completedItem.Name);
        Assert.True(completedItem.IsComplete);
    }
}