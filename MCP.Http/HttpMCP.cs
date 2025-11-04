using System.ComponentModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using ReGoMCP.Server.Endpoints;
namespace MCP.Http
{
    public static class HttpMCP
    {
        public async static Task Configure(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddConsole(consoleLogOptions =>
            {
                // Configure all logs to go to stderr
                consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowInspector", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            });

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
            });

            builder.Services.AddMcpServer()
                .WithHttpTransport()
                .WithToolsFromAssembly();

            //app.AddTodoEndpoint();
            var app = builder.Build();

            app.UseCors("AllowInspector");
            app.MapMcp();

            app.Run();
        }

        [McpServerToolType]
        public static class EchoTool
        {
            [McpServerTool, Description("Echoes the message back to the client.")]
            public static string Echo(string message) => $"hello {message}";
        }
    }
}
