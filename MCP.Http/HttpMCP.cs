using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using MCP.Http.Tools;

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

            var serverOptions = new McpServerOptions();
            serverOptions.ToolCollection = new McpServerPrimitiveCollection<McpServerTool>
            {
                McpServerTool.Create((string message) =>
                {
                    return $"hello from inline tool collection {message}.";
                }, new()
                {
                    Name = "echo",
                    Description = "Echoes the message back to the client."
                })
            };

            builder.Services.AddMcpServer()
                .WithHttpTransport()
                .WithToolsFromAssembly();

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
                .WithTracing(b => b.AddSource("*")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation())
                .WithMetrics(b => b.AddMeter("*")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                    {
                        metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
                    }))
                .WithLogging()
                .UseOtlpExporter();

            var app = builder.Build();

            app.UseCors("AllowInspector");
            app.MapMcp();

            app.Run();
        }
    }
}
