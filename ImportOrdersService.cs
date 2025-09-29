using GraphQL.Query.Builder;
using Microsoft.Extensions.Configuration;

namespace GraphQLExample;

public class ImportOrdersService(IGraphQlClient client,
    IConfiguration configuration) : IImportOrdersService
{
    private readonly string _uri = "https://graphql-gateway/graphql";
    private readonly string _filePath = @"c:\dev\Test\";
    private readonly string _idsFileFullPath = @"c:\dev\Test\ID-list.txt";
    private readonly string _jsonExtension = ".json";
    private readonly string _graphQlSchemaType = configuration.GetSection("ExternalAPI:SchemaType").Value ?? "TestType";
    private readonly string _graphQlQueryName = configuration.GetSection("ExternalAPI:QueryName").Value ?? "TestQuery";

    public async Task ImportOrder(string id)
    {
        var result = await GetJson(id);
        await SaveToFile(result,$"Result{_jsonExtension}");
    }

    public async Task ImportOrders()
    {
        var ids = await GetIds();
        int i = 0;
        foreach (var id in ids)
        {
            // give me just first 100 results
            if (i < 100)
            {
                i++;
            }
            else
            {
                break;
            }
            var result = await GetJson(id);
            await SaveToFile(result,$"{id}{_jsonExtension}");
        }
    }

    private async Task<IEnumerable<string>> GetIds()
    {
        if (!File.Exists(_idsFileFullPath))
        {
            return [];
        }
        return await File.ReadAllLinesAsync(_idsFileFullPath);
    }

    private async Task SaveToFile(string result, string filename)
    {
        var fullPath = $"{_filePath}{filename.Replace(":", "-")}";
        if (!Directory.Exists(_filePath))
        {
            Directory.CreateDirectory(_filePath);
        }
        await File.WriteAllTextAsync(fullPath, result);
    }

    private async Task<string> GetJson(string id)
    {
        IQuery<BusinessObject> queryBuilder = new Query<BusinessObject>(_graphQlSchemaType,
            new QueryOptions
            {
                Formatter = GraphQlCamelCaseFormatter.Format
            });

        queryBuilder
            .AddField(x => x.ObjectRef, q => q
                .AddField(x => x.Origin))
            .AddArgument(nameof(BusinessObject.Id).ToLower(), id);

        var query = queryBuilder.Build();
        var uri = configuration.GetSection("ExternalAPI:GraphQlUri").Value ?? _uri;
        return await client.Post(uri, query, _graphQlQueryName);
    }
}
