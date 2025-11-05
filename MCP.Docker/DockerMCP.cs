using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using MCP.Docker.Tools;

namespace MCP.Docker
{
    public static class DockerMCP
    {
        public async static Task Configure(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                //builder.Logging.AddConsole(consoleLogOptions =>
                //{
                //    // Configure all logs to go to stderr
                //    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
                //});

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowInspector", policy =>
                    {
                        policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                    });
                });

                builder.Services.AddMcpServer()
                    .WithHttpTransport()
                    .WithTools<ElasticSearchTool>();

                builder.Services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
                    .WithTracing(b => b.AddSource("*")
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter())
                    .WithMetrics(b => b.AddMeter("*")
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter((exporterOptions, metricReaderOptions) =>
                        {
                            metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000 * 60;
                        }))
                    .WithLogging()
                    .UseOtlpExporter();

                var app = builder.Build();

                app.UseCors("AllowInspector");
                app.MapMcp();

                app.Run();                
            }
            catch
            {

            }
        }
    }
}
