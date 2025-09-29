using System.Text.Json;

namespace GraphQLExample;

public class PropertiesAsLowercaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToLower();
}