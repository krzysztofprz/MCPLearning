using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.AspNetCore.Http;
using ModelContextProtocol.Server;
using System.ComponentModel;


namespace MCP.Docker.Tools
{
    [McpServerToolType]
    public class ElasticSearchTool
    {
        [McpServerTool(Name = "indices"), Description("Get indices form ElasticSearch cluster.")]
        public async static Task<IResult> GetIndices()
        {
            try
            {
                var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"));

                var client = new ElasticsearchClient(settings);

                var response = await client.Indices.GetAsync(new GetIndexRequest(Indices.All));

                return response.Indices.Keys.Count() > 0 ? Results.Ok(response.Indices.Keys.ToList()) : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.InternalServerError();
            }
        }
    }
}
