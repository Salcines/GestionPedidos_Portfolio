using GestionPedidos.Domain.Orders;

namespace GestionPedidos.UnitTests.Domain.Orders;

internal static class OrderMother
{
    private const int DEFAULT_QUANTITY = 1;
    private const decimal DEFAULT_UNIT_PRICE = 10.5m;
    
    public static Order Pending() => Order.Create(Guid.NewGuid());

    public static Order PendingWithItems()
    {
        var order = Pending();
        order.AddItem(Guid.NewGuid(), DEFAULT_QUANTITY, DEFAULT_UNIT_PRICE);
        return order;
    }

    public static Order Confirmed()
    {
        var order = PendingWithItems();
        order.Confirm();
        return order;
    }

    public static Order Shipped()
    {
        var order = Confirmed();
        order.Ship();
        return order;
    }
    
    public static Order Cancelled()
    {
        var order = PendingWithItems();
        order.Cancel();
        return order;
    }
}