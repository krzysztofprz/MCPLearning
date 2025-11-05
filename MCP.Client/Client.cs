using ModelContextProtocol.Client;
using OpenAI.Responses;

namespace MCP.Client
{
    public static class Client
    {
        public static async Task CreateStdio()
        {
            try
            {
                var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
                {
                    Command = "npx",
                    Arguments = ["-y", "@azure/mcp@latest", "server", "start"],
                    Name = "Azure MCP",
                });

                var client = await McpClient.CreateAsync(clientTransport);

                foreach (var tool in await client.ListToolsAsync())
                {
                    Console.WriteLine($"{tool.Name} ({tool.Description})");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async static Task CreateHttp(string mcpServerEndpoint)
        {
            try
            {
                var clientTransport = new HttpClientTransport(new()
                {
                    Endpoint = new Uri(mcpServerEndpoint)
                });

                var client = await McpClient.CreateAsync(clientTransport);

                foreach (var tool in await client.ListToolsAsync())
                {
                    Console.WriteLine($"{tool.Name} ({tool.Description})");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async static Task CreateOpenAI()
        {
            try
            {
                OpenAIResponseClient client = new(
                    model: "gpt-4.1",
                    apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

                OpenAIResponse response = client.CreateResponse("Tell me a a bedtime story about a unicorn and add current time.");

                Console.WriteLine(response.GetOutputText());
            }
            catch (Exception ex)
            {

            }
        }
    }
}
