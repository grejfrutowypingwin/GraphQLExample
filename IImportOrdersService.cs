namespace GraphQLExample;

public interface IImportOrdersService
{
    Task ImportOrder(string id);
    Task ImportOrders();
}