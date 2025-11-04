using Microsoft.AspNetCore.Http;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MCP.Http.Tools
{
    [McpServerToolType]
    public sealed class TodoTool
    {
        private static readonly Todo[] todos = [
            new(1, "Walk the dog"),
            new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
            new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
            new(4, "Clean the bathroom"),
            new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
            ];

        [McpServerTool, Description("Get list of todos.")]
        [McpMeta("category", "todo")]
        [McpMeta("dataSource", "Todo list")]
        public async Task<IResult> GetTodos()
        {
            return Results.Ok(todos);
        }

        [McpServerTool, Description("Get a todo for a given id.")]
        [McpMeta("category", "todo")]
        [McpMeta("dataSource", "Todo list")]
        public async Task<IResult> GetTodoById([Description("ID of the todo item from the list.")] int id)
        {
            return todos.FirstOrDefault(x => x.Id == id) is { } todo
                ? Results.Ok(todo)
                : Results.NotFound();
        }
    }

    public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

    [JsonSerializable(typeof(Todo[]))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {

    }
}
