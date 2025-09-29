using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;

namespace GraphQLExample;

public class GraphQlClient(IHttpClientFactory httpClientFactory) : IGraphQlClient
{
    public async Task<string> Post(string uri, string query, string queryName)
    {
        var queryObject = new GraphQlQueryObject($"query {queryName}{{{query}}}");

        var content = JsonContent.Create(queryObject,
            typeof(GraphQlQueryObject),
            new MediaTypeHeaderValue(MediaTypeNames.Application.Json),
            new JsonSerializerOptions { PropertyNamingPolicy = new PropertiesAsLowercaseNamingPolicy() });

        var httpClient = httpClientFactory.CreateClient(nameof(IGraphQlClient));
        var result = await httpClient.PostAsync(uri, content);
        return await result.Content.ReadAsStringAsync();
    }
}
