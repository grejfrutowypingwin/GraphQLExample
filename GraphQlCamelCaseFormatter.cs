using System.Reflection;

namespace GraphQLExample;

public class GraphQlCamelCaseFormatter
{
    // fix for camel casing filter words in default formatter
    private static readonly IReadOnlyCollection<string> IgnorePropertiesCaseSensitive = ["OR", "AND", "NOT"];

    public static Func<PropertyInfo, string> Format { get; } = (PropertyInfo property) =>
    {
        if (IgnorePropertiesCaseSensitive.Contains(property.Name))
        {
            return property.Name;
        }

        return char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);
    };
}
