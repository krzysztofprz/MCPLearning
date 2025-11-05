using MCP.Client;
using MCP.Docker;
using MCP.Http;
using MCP.Stdio;

// npx @modelcontextprotocol/inspector dotnet run --project "./ReGo.Main/ReGo.Main.csproj"
//await StdioMCP.Configure(args);

// npx @modelcontextprotocol/inspector --connect http://localhost:3001
//await HttpMCP.Configure(args);

// docker run -p 127.0.0.1:9200:9200 -d --name elasticsearch   -e "discovery.type=single-node"   -e "xpack.security.enabled=false"   -e "xpack.license.self_generated.type=trial"   -v "elasticsearch-data:/usr/share/elasticsearch/data"   docker.elastic.co/elasticsearch/elasticsearch:9.2.0
//await DockerMCP.Configure(args);

//await Client.CreateStdio();

//var microsoftLearn = "https://learn.microsoft.com/api/mcp";
//var gitHubCopilot = "https://api.githubcopilot.com/mcp/";

//await Client.CreateHttp(microsoftLearn);

//await Client.CreateOpenAI();