namespace GraphQLExample;

public interface IGraphQlClient
{
    Task<string> Post(string uri, string query, string queryName);
}