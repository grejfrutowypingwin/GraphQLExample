namespace GraphQLExample;

public class BusinessObject
{
    public string Id { get; set; } = null!;
    public ObjectRef ObjectRef { get; set; } = null!;
}

public class ObjectRef
{
    public string Origin { get; set; } = null!;
}
